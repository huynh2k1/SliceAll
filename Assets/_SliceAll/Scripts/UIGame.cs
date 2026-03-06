using System;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : BaseUI
{
    public override UIType Type => UIType.GAME;
    [SerializeField] Button _btnPause;
    [SerializeField] GameObject _cross;
    [SerializeField] Image _btnShoot;

    public static Action OnClickBtnPauseAction;

    private void Awake()
    {
        _btnPause.onClick.AddListener(OnBtnPauseClicked);
    }

    private void OnEnable()
    {
        BtnShoot.OnPointerDownAction += OnAimState;
        BtnShoot.OnPointerUpAction += OnNormalState;
    }

    private void OnDisable()
    {
        BtnShoot.OnPointerDownAction -= OnAimState;
        BtnShoot.OnPointerUpAction -= OnNormalState;
    }

    void OnNormalState()
    {
        _cross.SetActive(false);
        _btnShoot.enabled = true;
    }

    void OnAimState()
    {
        _cross.SetActive(true);
        _btnShoot.enabled = false;
    }

    void OnBtnPauseClicked()
    {
        OnClickBtnPauseAction?.Invoke();
    }
}
