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


    [SerializeField] Slider _sliderSound;
    [SerializeField] Slider _sliderMusic;

    public static Action OnClickBtnHomeAction;
    public static Action OnClickBtnResumeAction;

    protected override void Awake()
    {
        base.Awake();
        _btnHome.onClick.AddListener(OnBtnHomeClicked);
        _btnResume.onClick.AddListener(OnBtnResumeClicked);

        _sliderMusic.onValueChanged.AddListener((v) =>
        {
            OnVolumeMusicChange(v);
        });
        _sliderSound.onValueChanged.AddListener((v) =>
        {
            OnVolumeSoundChange(v);
        });
    }

    public void OnBtnHomeClicked()
    {
        Hide(() =>
        {
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

    public override void Show()
    {
        base.Show();
        Load();
    }

    void Load()
    {
        _sliderSound.value = DataPrefs.Sound;
        _sliderMusic.value = DataPrefs.Music;
    }

    void OnVolumeSoundChange(float value)
    {
        DataPrefs.Sound = value;
        //SoundCtrl.I.OnVolumeSoundChange();
    }

    void OnVolumeMusicChange(float value)
    {
        DataPrefs.Music = value;
        SoundCtrl.I.OnVolumeMusicChange();
    }

}
