using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    List<PathCollider> pathColliders;

    public void Init()
    {
        //sort on z or something 
    }

    void AddToList(PathCollider col)
    {
        if(pathColliders == null)
            pathColliders = new List<PathCollider>();

        pathColliders.Add(col);
    }

    public Bounds GetFirstBoundaryCollision(Vector3[] path, int maxIndex)
    {

    }

}
