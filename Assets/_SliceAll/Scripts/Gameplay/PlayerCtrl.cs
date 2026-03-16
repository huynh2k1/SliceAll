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

    bool _canShoot = false;

    Coroutine _aimBlendRoutine;
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
        if (_aimBlendRoutine != null)
        {
            StopCoroutine(_aimBlendRoutine);
            _aimBlendRoutine = null;

            _vitualCam.enabled = false;
            _animator.SetBool("Aim", false);
            transform.rotation = _initRotation;

            OnNormalStateAction?.Invoke();

            return;
        }

        if (_canShoot)
        {
            Shoot();
        }

        _canShoot = false;

        StartCoroutine(WaitForBlendCamComplete());
    }

    IEnumerator WaitForBlendCamComplete()
    {
        _animator.SetBool("Aim", false);
        yield return new WaitForSeconds(1f);
        _vitualCam.enabled = false;
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

        _canShoot = false;
        SoundCtrl.I.PlaySFXByType(TypeSFX.DRAGBOW);
        _aimBlendRoutine = StartCoroutine(WaitForAimBlendComplete());
    }

    IEnumerator WaitForAimBlendComplete()
    {
        yield return null;

        while (_cinemachineBrain.IsBlending)
        {
            yield return null;
        }
            
        _canShoot = true;
        if (_aimBlendRoutine != null)
        {
            StopCoroutine(_aimBlendRoutine);
            _aimBlendRoutine = null;
        }

    }

    public void Shoot()
    {
        SoundCtrl.I.PlaySFXByType(TypeSFX.SHOOT);
        Bullet b = Instantiate(_bulletPrefab, _firePos.position, _firePos.rotation);
    }
}
