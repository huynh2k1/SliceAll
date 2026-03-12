using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] Animator _animator;

    [SerializeField] CinemachineBrain _cinemachineBrain;
    [SerializeField] CinemachineVirtualCamera _vitualCam;

    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] Transform _firePos;

    float _timeBlend = 0.4f;
    Quaternion _initRotation;

    public static Action OnNormalStateAction;
    public static Action OnPlayerInit;


    private void Awake()
    {
        _cinemachineBrain.m_DefaultBlend.m_Time = _timeBlend;
    }

    public void Init(Quaternion rotation)
    {
        transform.rotation = rotation;
        _initRotation = rotation;
        OnPlayerInit?.Invoke();
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

    [Button("Normal")]
    public void OnNormalState()
    {
        Shoot();

        StartCoroutine(WaitForBlendCamComplete());
    }

    IEnumerator WaitForBlendCamComplete()
    {
        yield return new WaitForSeconds(1f);
        _vitualCam.enabled = false;
        _animator.SetBool("Aim", false);
        transform.rotation = _initRotation;
        yield return new WaitForSeconds(_timeBlend);
        //
        OnNormalStateAction?.Invoke();
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
