using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public EnemyType Type;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody[] _rigidbodies;
    [SerializeField] Collider[] _colliders;
    public bool IsDead { get; protected set; }


    [Button("Setup Ragdoll")]
    public void SetUpRagdoll()
    {
        if (_rigidbodies == null || _rigidbodies.Length == 0)
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
        }
        if (_colliders == null || _colliders.Length == 0)
        {
            _colliders = GetComponentsInChildren<Collider>();
        }
    }

    protected virtual void Awake()
    {
        if (_rigidbodies == null || _rigidbodies.Length == 0)
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
        }
        if (_colliders == null || _colliders.Length == 0)
        {
            _colliders = GetComponentsInChildren<Collider>();
        }
    }

    protected virtual void EnableRagdoll(bool isEnable)
    {
        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = isEnable;
        }
    }

    public virtual void Dead()
    {
        IsDead = true;
        _animator.enabled = false;
        EnableRagdoll(false);
    }
}
