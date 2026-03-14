using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 30f;
    const float pushForce = 50f;

    Rigidbody _rb;
    bool _stopped;

    [SerializeField] ParticleSystem _hitEffect;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    void OnEnable()
    {
        _stopped = false;

        _rb.isKinematic = false;
        _rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_stopped) return;
        _stopped = true;
        Debug.Log(other.name);
        SpawnHitEffect(transform.position);

        BaseObstacle obstacle = other.GetComponent<BaseObstacle>();
        if (obstacle != null)
        {
            obstacle.OnCollisionWithBullet();
        }

        BaseEnemy e = other.GetComponentInParent<BaseEnemy>();
        if (e != null)
        {
            e.Dead();
            Rigidbody hitRb = other.attachedRigidbody;
            if (hitRb != null)
            {
                hitRb.AddForce(transform.forward * pushForce, ForceMode.Impulse);
            }

        }
        Destroy(gameObject);
    }

    void SpawnHitEffect(Vector3 pos)
    {
        ParticleSystem p = Instantiate(_hitEffect, pos, Quaternion.identity);
        p.transform.localScale = Vector3.one * 0.5f;
        p.Play();
    }
}