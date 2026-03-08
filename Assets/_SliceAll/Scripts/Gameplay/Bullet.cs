using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 30f;

    Rigidbody _rb;
    bool _stopped;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        // Không chịu trọng lực
        _rb.useGravity = false;
    }

    void OnEnable()
    {
        _stopped = false;

        _rb.isKinematic = false;
        _rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_stopped) return;

        _stopped = true;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // dừng hẳn lại
        _rb.isKinematic = true;

        BaseObstacle o = collision.gameObject.GetComponent<BaseObstacle>();
        if(o != null)
        {
            o.OnCollisionWithBullet();
        }

        //Checking Enemy
        BaseEnemy e = collision.gameObject.GetComponentInParent<BaseEnemy>();
        if (e != null)
        {
            transform.SetParent(collision.transform.parent);
            e.Dead();
        }
    }
}