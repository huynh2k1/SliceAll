using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : BaseUICtrl
{
    public void OnInitGame()
    {
        Show(UIType.HOME);
    }

    public void OnGameHome()
    {
        Show(UIType.HOME);
        Hide(UIType.GAME);
    }

    public void OnStartGame()
    {
        Hide(UIType.HOME);
        Show(UIType.GAME);
    }

    public void OnPauseGame()
    {
        Show(UIType.PAUSE);
    }

    public void OnWinGame()
    {
        Hide(UIType.GAME);
        Show(UIType.WIN);
    }

    public void OnLoseGame()
    {
        Hide(UIType.GAME);
        Show(UIType.LOSE);
    }


}
