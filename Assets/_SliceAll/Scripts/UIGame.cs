using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField] GameObject _cross;
    [SerializeField] Image _btnShoot;

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
}
