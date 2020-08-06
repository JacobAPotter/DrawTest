using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager ActiveGameManager;

    public Camera MainCamera { get; private set; }
    public Player Player1 { get; private set; }
    public TimeManager TimeManager { get; private set; }
    public InputManager InputManager { get; private set; }
    public StateManager StateManager { get; private set; }
    public Text DebugText { get; private set; }

    void Awake()
    {
        ActiveGameManager = this;

        Transform world = GameObject.Find("World").transform;

        MainCamera = world.Find("MainCamera").GetComponent<Camera>();
        TimeManager = world.Find("TimeManager").GetComponent<TimeManager>();
        InputManager = world.Find("InputManager").GetComponent<InputManager>();
        DebugText = world.Find("Canvas").Find("Debug").GetComponent<Text>();

        Player1 = world.Find("Player").GetComponent<Player>();
        StateManager = Player1.GetComponent<StateManager>();


    }
}
