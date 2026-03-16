using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : BasePopup
{
    public override UIType Type => UIType.SETTING;

    [SerializeField] Slider _sliderSound;
    [SerializeField] Slider _sliderMusic;

    protected override void Awake()
    {
        base.Awake();
        _sliderMusic.onValueChanged.AddListener((v) =>
        {
            OnVolumeMusicChange(v);
        });
        _sliderSound.onValueChanged.AddListener((v) =>
        {
            OnVolumeSoundChange(v);
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
