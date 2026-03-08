using UnityEngine;

public class Barrel : BaseObstacle
{
    [SerializeField] float radius = 5f;
    [SerializeField] float force = 700f;
    [SerializeField] float upwards = 2f;
    [SerializeField] LayerMask hitLayer;

    bool _exploded;
    public override void OnCollisionWithBullet()
    {
        base.OnCollisionWithBullet();
        Bang();
    }
    public void Bang()
    {
        if (_exploded) return;
        _exploded = true;

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
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}