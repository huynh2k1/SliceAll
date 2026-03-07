using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUICtrl : MonoBehaviour
{
    public BaseUI[] _uis;
    protected Dictionary<UIType, BaseUI> _dictUIs = new Dictionary<UIType, BaseUI>();

    [Header("Loading Setup")]
    public Image Mask;
    public float timeFade = 0.3f;
    public float timeLoading = 1f;

#if UNITY_EDITOR
    [Button("Load All UI")]
    public void LoadAllUIs()
    {
        _uis = GetComponentsInChildren<BaseUI>(true);
    }
#endif


    protected virtual void Awake()
    {
        foreach (BaseUI ui in _uis)
        {
            if (_dictUIs.ContainsKey(ui.Type) == false)
            {
                _dictUIs.Add(ui.Type, ui);
            }
        }
    }

    public void Show(UIType type)
    {
        if (_dictUIs.ContainsKey(type) == false)
        {
            return;
        }

        _dictUIs[type].Show();
    }

    public void Hide(UIType type)
    {
        if (_dictUIs.ContainsKey(type) == false)
        {
            return;
        }

        _dictUIs[type].Hide();
    }

    public void FadeMask(Action actionDone1 = default, Action actionDone2 = default)
    {
        Mask.DOKill();
        Mask.DOFade(1, timeFade).From(0).SetUpdate(true).SetEase(Ease.Linear).OnComplete(() =>
        {
            actionDone1?.Invoke();
            Mask.DOFade(0, timeFade).SetDelay(timeLoading).SetUpdate(true).SetEase(Ease.Linear).OnComplete(() =>
            {
                actionDone2?.Invoke();
            });
        });
    }

}
