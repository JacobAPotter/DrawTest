using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashManager : MonoBehaviour
{
    Vector3 startPoint;
    InputManager inputManager;
    LineRenderer lineRenderer;
    StateManager stateManager;
    CharacterController characterController;
    const float dashDistance = 8;
    public bool Complete { get; private set; }

    private void Start()
    {
        inputManager = GameManager.ActiveGameManager.InputManager;
        lineRenderer = GetComponent<LineRenderer>();
        stateManager = GameManager.ActiveGameManager.StateManager;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
                Vector3 newPoint;
                if (stateManager.RaycastMouse(out newPoint))
                {
                    if (!lineRenderer.enabled)
                        lineRenderer.enabled = true;

                    lineRenderer.SetPosition(1, newPoint);
                }

                if (!inputManager.PathMouseHeld)
                {
                    characterController.Move((newPoint - startPoint).normalized * dashDistance);
                    Complete = true;
                    lineRenderer.enabled = false;
                }
    }

    public void Init(Vector3 startPos)
    {
        Complete = false;
        lineRenderer.positionCount = 2;
        //dont enable it until we have a second point 
        //to set
        lineRenderer.enabled = false;
        startPoint = startPos;
        lineRenderer.SetPosition(0, startPos);
    }
}
