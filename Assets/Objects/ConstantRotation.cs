﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [SerializeField]
    Vector3 rotation;
    TimeManager timeManager;
    private void Start()
    {
        timeManager = GameManager.ActiveGameManager.TimeManager;
    }
    void Update()
    {
        transform.Rotate(rotation * timeManager.WorldDeltaTime);
    }
}
