using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstUtils
{
    public const string CUR_LEVEL = "CUR_LEVEL";

    //SFX
    public const string SOUND = "SOUND";
    public const string MUSIC = "MUSIC";
    public const string VIBRATE = "VIBRATE";
}

public class DataPrefs
{
    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt(ConstUtils.CUR_LEVEL, 0);
        set => PlayerPrefs.SetInt(ConstUtils.CUR_LEVEL, value);
    }

    public static bool IsSoundOn
    {
        get => PlayerPrefs.GetInt(ConstUtils.SOUND, 0) == 0 ? true : false;
        set => PlayerPrefs.SetInt(ConstUtils.SOUND, value ? 0 : 1);
    }

    public static bool IsMusicOn
    {
        get => PlayerPrefs.GetInt(ConstUtils.MUSIC, 0) == 0 ? true : false;
        set => PlayerPrefs.SetInt(ConstUtils.MUSIC, value ? 0 : 1);
    }

    public static bool IsVibrateOn
    {
        get => PlayerPrefs.GetInt(ConstUtils.VIBRATE, 0) == 0 ? true : false;
        set => PlayerPrefs.SetInt(ConstUtils.VIBRATE, value ? 0 : 1);
    }
}
