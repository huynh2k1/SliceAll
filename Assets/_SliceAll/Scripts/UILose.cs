using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILose : BasePopup
{
    public override UIType Type => UIType.LOSE;

    [SerializeField] Button _btnHome;
    [SerializeField] Button _btnReplay;

    public static Action OnClickBtnHome;
    public static Action OnClickBtnReplay;

    protected override void Awake()
    {
        base.Awake();
        _btnHome.onClick.AddListener(OnClickHome);
        _btnReplay.onClick.AddListener(OnClickReplay);
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
