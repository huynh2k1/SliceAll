using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    public static GameCtrl I;
    [SerializeField] UICtrl _uiCtrl;
    public enum GameState
    {
        NONE,
        PLAYING,
    }

    GameState curState;

    private void Awake()
    {
        Application.targetFrameRate = 120;
        I = this;
    }

    private void OnEnable()
    {
        UIHome.OnClickBtnPlay += OnStartGame;
        UIHome.OnClickBtnSetting += OnSettingGame;

        UIGame.OnClickBtnPauseAction += OnPauseGame;

        UIPause.OnClickBtnResumeAction += OnResumeGame;
        UIPause.OnClickBtnHomeAction += OnGameHome;
    }

    private void OnDestroy()
    {
        UIHome.OnClickBtnPlay -= OnStartGame;
        UIHome.OnClickBtnSetting -= OnSettingGame;

        UIGame.OnClickBtnPauseAction -= OnPauseGame;

        UIPause.OnClickBtnResumeAction -= OnResumeGame;
        UIPause.OnClickBtnHomeAction -= OnGameHome;
    }

    private void Start()
    {
        OnInitGame();   
    }

    public void ChangeState(GameState newState)
    {
        curState = newState;    
    }

    public void OnInitGame()
    {
        ChangeState(GameState.NONE);
        _uiCtrl.OnInitGame();
    }

    public void OnGameHome()
    {
        Loading(() =>
        {
            ChangeState(GameState.NONE);
            _uiCtrl.OnGameHome();
        });
    }
    
    public void OnStartGame()
    {
        Loading(() =>
        {
            _uiCtrl.OnStartGame();
        }, () =>
        {
            ChangeState(GameState.PLAYING);
        });
    }

    public void OnWinGame()
    {
        ChangeState(GameState.NONE);
    }

    public void OnLoseGame()
    {
        ChangeState(GameState.NONE);

    }

    public void OnPauseGame()
    {
        ChangeState(GameState.NONE);
        Time.timeScale = 0;
        _uiCtrl.OnPauseGame();
    }

    public void OnResumeGame()
    {
        ChangeState(GameState.NONE);
        Time.timeScale = 1;
    }

    public void OnReplayGame()
    {
        ChangeState(GameState.NONE);

    }

    public void OnSettingGame()
    {
        _uiCtrl.Show(UIType.SETTING);
    }

    void Loading(Action action1 = default, Action action2 = default)
    {
        _uiCtrl.FadeMask(action1, action2);
    }
}

