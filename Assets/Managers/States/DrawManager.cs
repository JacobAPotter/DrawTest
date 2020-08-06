using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{

    public enum PATH_STATE
    {
        DRAWING,
        ON_PATH,
        POST,
        INACTIVE,
    }

    public PATH_STATE CURRENT_DRAW_STATE { get; private set; }

    Player player;
    Camera mainCamera;
    InputManager inputManager;
    int layerMask;

    int drawPointIndex;
    Vector3[] drawPoints;
    const int maxDrawPoints = 300;
    const float maxDrawPointDistance = 2f;
    float drawTimeStamp;
    const float maxDrawTime = 3f;
    float accumDrawDistance;

    int smoothedPointIndex;
    const int maxSmoothedPoints = 600;
    const float minSmoothPointDistance = 0.5f;
    const float minSmoothPointDistanceSquared = minSmoothPointDistance * minSmoothPointDistance;

    //the maximum length we can create a smooth curve for with some wiggle room
    const float maxAccumDrawDistance = maxSmoothedPoints * minSmoothPointDistanceSquared * 0.98f;

    Vector3[] smoothedPoints;
    LineRenderer lineRenderer;
    TimeManager timeManager;

    float onPathTimeStamp;

    StateManager stateManager;

    private void Start()
    {
        player = GameManager.ActiveGameManager.Player1;
        mainCamera = GameManager.ActiveGameManager.MainCamera;
        layerMask = LayerMask.GetMask("PathCol");
        lineRenderer = GetComponent<LineRenderer>();
        smoothedPoints = new Vector3[maxSmoothedPoints];
        drawPoints = new Vector3[maxDrawPoints];
        timeManager = GameManager.ActiveGameManager.TimeManager;
        stateManager = GetComponent<StateManager>();
        inputManager = GameManager.ActiveGameManager.InputManager;
        Deactivate();
    }

    private void Update()
    {
        switch (CURRENT_DRAW_STATE)
        {
            case PATH_STATE.DRAWING:
                {
                    timeManager.SlowTime();

                    //Theyre drawing a path. The player will move to the first point on the path. 
                    if (inputManager.PathMouseHeld &&
                        timeManager.WorldTime - drawTimeStamp < maxDrawTime &&
                        drawPointIndex < maxDrawPoints - 1 &&
                        accumDrawDistance < maxAccumDrawDistance)
                    {
                        Vector3 newPoint;

                        if (stateManager.RaycastMouse(out newPoint))
                        {
                            float distFromLastPoint = Vector3.Distance(drawPoints[drawPointIndex], newPoint);

                            if (distFromLastPoint > maxDrawPointDistance)
                            {
                                if (drawPointIndex + 2 >= maxDrawPoints)
                                {
                                    //last point, make sure it does not cause the path to 
                                    //exceed the maximum distance.
                                    if (accumDrawDistance + distFromLastPoint > maxDrawPointDistance)
                                    {
                                        //find the max distance and normalize the difference to that
                                        float maxdistToNewPoint = maxDrawPointDistance - accumDrawDistance;
                                        Vector3 direction = newPoint - drawPoints[drawPointIndex];
                                        newPoint = drawPoints[drawPointIndex] + (direction.normalized * maxDrawPointDistance);
                                    }
                                }

                                accumDrawDistance += distFromLastPoint;
                                drawPoints[++drawPointIndex] = newPoint;
                                UpdateLineRenderer(drawPoints, drawPointIndex);
                            }
                            else //just so we can see the line renderer point to the mouse position
                            {
                                drawPoints[drawPointIndex + 1] = newPoint;
                                drawPoints[drawPointIndex + 2] = newPoint;
                                UpdateLineRenderer(drawPoints, drawPointIndex + 2);
                            }
                        }
                    }
                    else
                    {
                        GenerateSmoothPath();

                        //Were ready to begin going down the path
                        CURRENT_DRAW_STATE = PATH_STATE.ON_PATH;
                        onPathTimeStamp = timeManager.WorldTime;
                    }


                }
                break;

            case PATH_STATE.ON_PATH:
                {
                    UpdateLineRenderer(smoothedPoints, smoothedPointIndex);
                    timeManager.SlowTime();

                    float maxPathTime = Mathf.Min(accumDrawDistance * 0.005f, 0.35f);

                    if (timeManager.WorldTime - onPathTimeStamp > maxPathTime)
                    {
                        CURRENT_DRAW_STATE = PATH_STATE.POST;
                        transform.position = smoothedPoints[smoothedPointIndex] + Vector3.up * player.PlayerBounds.extents.y;
                    }
                }

                break;
            case PATH_STATE.POST:

                if (lineRenderer.positionCount > 0)
                {
                    lineRenderer.positionCount -= 1;
                }
                else
                    Deactivate();

                break;
        }
    }

    public void Init(Vector3 startingPoint)
    {
        CURRENT_DRAW_STATE = PATH_STATE.DRAWING;
        drawPointIndex = 0;
        drawPoints[drawPointIndex] = startingPoint;
        drawTimeStamp = timeManager.WorldTime;
        lineRenderer.enabled = true;
    }

    void GenerateSmoothPath()
    {
        for (int i = 0; i <= drawPointIndex; i++)
            smoothedPoints[i] = drawPoints[i];

        smoothedPointIndex = drawPointIndex;
    }

    void Deactivate()
    {
        CURRENT_DRAW_STATE = PATH_STATE.INACTIVE;
        drawPointIndex = -1;
        smoothedPointIndex = -1;
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;
        accumDrawDistance = 0;
        this.enabled = false;
    }

    void UpdateLineRenderer(Vector3[] points, int maxIndex)
    {
        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;

        Vector3[] newPoints = new Vector3[maxIndex];

        Vector3 aboveGround = Vector3.up * 0.08f;

        for (int i = 0; i < newPoints.Length; i++)
            newPoints[i] = points[i] + aboveGround;

        lineRenderer.positionCount = maxIndex;
        lineRenderer.SetPositions(newPoints);
    }

}
