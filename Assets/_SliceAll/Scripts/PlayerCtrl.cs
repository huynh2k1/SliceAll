using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] Animator _animator;

    [SerializeField] CinemachineVirtualCamera _vitualCam;

    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] Transform _firePos;

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
        Shoot();
        DOVirtual.DelayedCall(1f, () =>
        {
            _vitualCam.enabled = false;
            _animator.SetBool("Aim", false);
            transform.rotation = Quaternion.Euler(Vector3.zero);
        });
    }

    [Button("Aim")]
    public void OnAimState()
    {
        _vitualCam.enabled = true;
        _animator.SetBool("Aim", true);
    }

    public void Shoot()
    {
        Bullet b = Instantiate(_bulletPrefab, _firePos.position, _firePos.rotation);
    }
}
