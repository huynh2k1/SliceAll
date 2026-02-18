using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    public static GameCtrl I;

    GameState curState;

    private void Awake()
    {
        Application.targetFrameRate = 120;
        I = this;
    }

    private void Start()
    {
        OnInit();   
    }

    public void ChangeState(GameState newState)
    {
        curState = newState;    
    }

    public void OnInit()
    {
        ChangeState(GameState.NONE);
        Debug.Log("Welcome 2026");
    }
    public enum GameState
    {
        NONE,
        PLAYING,
    }
}

