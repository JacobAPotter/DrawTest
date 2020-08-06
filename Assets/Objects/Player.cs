using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public  Bounds PlayerBounds { get; private set; }
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        PlayerBounds = rend.bounds;
    }
}
