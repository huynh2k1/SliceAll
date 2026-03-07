using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : BaseUI
{
    public override UIType Type => UIType.HOME;

    [SerializeField] Button _btnPlay;
    [SerializeField] Button _btnSetting;

    public static Action OnClickBtnPlay;
    public static Action OnClickBtnSetting;

    public void Start()
    {
        _btnPlay.onClick.AddListener(OnBtnPlayClicked);
        _btnSetting.onClick.AddListener(OnBtnSettingClicked);
    }   

    void OnBtnPlayClicked()
    {
        OnClickBtnPlay?.Invoke();
    }

    void OnBtnSettingClicked()
    {
        OnClickBtnSetting?.Invoke();    
    }

}
