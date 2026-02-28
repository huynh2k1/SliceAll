using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    public abstract UIType Type { get;}

    public virtual void Show() => gameObject.SetActive(true);
    public virtual void Hide() => gameObject.SetActive(false);

}
public enum UIType
{
    HOME,
    GAME,
    SETTING,
    PAUSE,
    WIN,
    LOSE,
    SHOP
}

