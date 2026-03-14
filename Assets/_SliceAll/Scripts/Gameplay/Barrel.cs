using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Barrel : BaseObstacle
{
    [SerializeField] float radius = 5f;
    [SerializeField] float force = 700f;
    [SerializeField] float upwards = 2f;
    [SerializeField] LayerMask hitLayer;

    [SerializeField] ParticleSystem _explosionPrefab;

    public static Action OnBangAction;

    bool _exploded;
    public override void OnCollisionWithBullet()
    {
        Debug.Log("Collision With Bullet");
        Bang();
    }

    public void Bang()
    {
        if (_exploded) return;
        _exploded = true;

        OnBangAction?.Invoke();
        //Spawn fx
        ParticleSystem fx = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        fx.Play();

        DOVirtual.DelayedCall(0.2f, () =>
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, hitLayer);

            foreach (Collider hit in hits)
            {
                // ================= ENEMY =================
                BaseEnemy enemy = hit.GetComponentInParent<BaseEnemy>();
                if (enemy != null)
                {
                    enemy.Dead();
                }

                // ================= PUSH =================
                Rigidbody rb = hit.attachedRigidbody;
                if (rb != null)
                {
                    rb.AddExplosionForce(force, transform.position, radius, upwards, ForceMode.Impulse);
                }
            }

            Destroy(gameObject);
        });

    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}