using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    public static GameCtrl I;
    [SerializeField] UICtrl _uiCtrl;
    [SerializeField] LevelCtrl _levelCtrl;
    public enum GameState
    {
        NONE,
        PLAYING,
    }

    GameState curState;

    private void Awake()
    {
        I = this;
    }

    private void OnEnable()
    {
        UIHome.OnClickBtnPlay += OnStartGame;
        UIHome.OnClickBtnSetting += OnSettingGame;

        UIGame.OnClickBtnPauseAction += OnPauseGame;

        UIPause.OnClickBtnResumeAction += OnResumeGame;
        UIPause.OnClickBtnHomeAction += OnGameHome;

        UIWin.OnClickBtnHome += OnGameHome;
        UIWin.OnClickBtnNext += OnNextGame;
        UIWin.OnClickBtnReplay += OnReplayGame;

        _levelCtrl.OnClearEnemyAction += OnWinGame;
    }

    private void OnDestroy()
    {
        UIHome.OnClickBtnPlay -= OnStartGame;
        UIHome.OnClickBtnSetting -= OnSettingGame;

        UIGame.OnClickBtnPauseAction -= OnPauseGame;

        UIPause.OnClickBtnResumeAction -= OnResumeGame;
        UIPause.OnClickBtnHomeAction -= OnGameHome;

        UIWin.OnClickBtnHome -= OnGameHome;
        UIWin.OnClickBtnNext -= OnNextGame;
        UIWin.OnClickBtnReplay -= OnReplayGame;

        _levelCtrl.OnClearEnemyAction -= OnWinGame;
    }

    public void ChangeState(GameState newState)
    {
        curState = newState;    
    }

    public void OnInitGame()
    {
        SoundCtrl.I.PlayMusic();
        ChangeState(GameState.NONE);
        _uiCtrl.OnInitGame();
    }

    public void OnGameHome()
    {
        Loading(() =>
        {

            _levelCtrl.DestroyCurLevel();
            ChangeState(GameState.NONE);
            _uiCtrl.OnGameHome();
            Time.timeScale = 1;
        });
    }
    
    public void OnStartGame()
    {
        Loading(() =>
        {
            Time.timeScale = 1;
            _uiCtrl.OnStartGame();
            _levelCtrl.InitLevel(DataPrefs.CurrentLevel);
        }, () =>
        {
            ChangeState(GameState.PLAYING);
        });
    }

    public void OnWinGame()
    {
        ChangeState(GameState.NONE);
        _uiCtrl.OnWinGame();
        SoundCtrl.I.PlaySFXByType(TypeSFX.WIN);
    }

    public void OnLoseGame()
    {
        ChangeState(GameState.NONE);
        _uiCtrl.OnLoseGame();
        SoundCtrl.I.PlaySFXByType(TypeSFX.LOSE);
    }

    public void OnPauseGame()
    {
        ChangeState(GameState.NONE);
        _uiCtrl.OnPauseGame();
    }

    public void OnResumeGame()
    {
        ChangeState(GameState.NONE);
        Time.timeScale = 1;
    }

    public void OnNextGame()
    {
        _levelCtrl.OnNextGame();
        OnStartGame();
    }

    public void OnReplayGame()
    {
        OnStartGame();
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

