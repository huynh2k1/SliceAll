using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] Animator _animator;

    [SerializeField] CinemachineVirtualCamera _vitualCam;

    private void OnEnable()
    {
        BtnShoot.OnPointerDownAction += OnAimState;
        BtnShoot.OnPointerUpAction += OnNormalState;
    }

    private void OnDestroy()
    {
        BtnShoot.OnPointerDownAction -= OnAimState;
        BtnShoot.OnPointerUpAction -= OnNormalState;
    }

    [Button("Normal")]
    public void OnNormalState()
    {
        _vitualCam.enabled = false;
        _animator.SetBool("Aim", false);
    }

    [Button("Aim")]
    public void OnAimState()
    {
        _vitualCam.enabled = true;
        _animator.SetBool("Aim", true);
    }

    public void Shoot()
    {

    }
}
