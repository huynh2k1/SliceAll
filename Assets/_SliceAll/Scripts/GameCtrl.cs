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
    }

    private void OnDestroy()
    {
        UIHome.OnClickBtnPlay -= OnStartGame;
        UIHome.OnClickBtnSetting -= OnSettingGame;

        UIGame.OnClickBtnPauseAction -= OnPauseGame;
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
    
    public void OnStartGame()
    {
        ChangeState(GameState.PLAYING);
        _uiCtrl.OnStartGame();
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

    }

    public void OnResumeGame()
    {
        ChangeState(GameState.NONE);

    }

    public void OnReplayGame()
    {
        ChangeState(GameState.NONE);

    }

    public void OnSettingGame()
    {
        _uiCtrl.Show(UIType.SETTING);
    }
}

