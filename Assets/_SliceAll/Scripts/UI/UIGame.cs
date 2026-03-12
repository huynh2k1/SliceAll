using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : BaseUI
{
    public override UIType Type => UIType.GAME;
    [SerializeField] Button _btnPause;
    [SerializeField] GameObject _cross;
    [SerializeField] Image _btnShoot;

    [SerializeField] TMP_Text _txtEnemyCount;
    [SerializeField] TMP_Text _txtArrowCount;
    [SerializeField] TMP_Text _txtLevel;

    public static Action OnClickBtnPauseAction;

    private void Awake()
    {
        _btnPause.onClick.AddListener(OnBtnPauseClicked);
        BtnShoot.OnPointerDownAction += OnAimState;
        BtnShoot.OnPointerUpAction += OnPointerUp;
        PlayerCtrl.OnNormalStateAction += OnNormalState;

        LevelCtrl.OnEnemyAliveChange += UpdateTextEnemyCount;
    }

    public override void Show()
    {
        base.Show();
        UpdateTextLevel();
    }

    private void OnDestroy()
    {
        BtnShoot.OnPointerDownAction -= OnAimState;
        BtnShoot.OnPointerUpAction -= OnPointerUp;  
        PlayerCtrl.OnNormalStateAction -= OnNormalState;

        LevelCtrl.OnEnemyAliveChange -= UpdateTextEnemyCount;
    }

    void OnNormalState()
    {
        _cross.SetActive(false);
        _btnShoot.enabled = true;
    }

    void OnPointerUp()
    {
        _cross.SetActive(false);
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

    void UpdateTextEnemyCount(int count)
    {
        _txtEnemyCount.text = count.ToString(); 
    }

    void UpdateTextArrowCount(int count)
    {
        _txtArrowCount.text = count.ToString();
    }

    void UpdateTextLevel()
    {
        _txtLevel.text = $"LEVEL {DataPrefs.CurrentLevel + 1}";
    }
}
