using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : BasePopup
{
    public override UIType Type => UIType.PAUSE;

    [SerializeField] Button _btnHome;
    [SerializeField] Button _btnResume;

    public static Action OnClickBtnHomeAction;
    public static Action OnClickBtnResumeAction;

    protected override void Awake()
    {
        base.Awake();
        _btnHome.onClick.AddListener(OnBtnHomeClicked);
        _btnResume.onClick.AddListener(OnBtnResumeClicked);
    }

    public void OnBtnHomeClicked()
    {
        Hide(() =>
        {
            Time.timeScale = 1; 
            OnClickBtnHomeAction?.Invoke(); 
        });
    }

    public void OnBtnResumeClicked()
    {
        Hide(() =>
        {
            OnClickBtnResumeAction?.Invoke();
        });
    }



}
