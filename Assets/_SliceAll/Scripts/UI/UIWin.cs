using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : BasePopup
{
    public override UIType Type => UIType.WIN;

    [SerializeField] Button _btnHome;
    //[SerializeField] Button _btnReplay;
    [SerializeField] Button _btnNext;

    public static Action OnClickBtnHome;
    public static Action OnClickBtnReplay;
    public static Action OnClickBtnNext;

    protected override void Awake()
    {
        base.Awake();
        _btnHome.onClick.AddListener(OnClickHome);
        //_btnReplay.onClick.AddListener(OnClickReplay);
        _btnNext.onClick.AddListener(OnClickNext);
    }

    public void OnClickNext()
    {
        Hide(() =>
        {
            OnClickBtnNext?.Invoke();
        });
    }   

    public void OnClickReplay()
    {
        Hide(() =>
        {
            OnClickBtnReplay?.Invoke();
        });
    }

    public void OnClickHome()
    {
        Hide(() =>
        {
            OnClickBtnHome?.Invoke();
        });
    }
}
