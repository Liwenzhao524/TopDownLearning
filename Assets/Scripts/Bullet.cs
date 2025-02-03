using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody _rb => GetComponent<Rigidbody>();

    const float DEFAULT_BULLETSPEED = 20;

    public void SetupBullet(Vector3 velocity)
    {
        _rb.velocity = velocity;
        _rb.mass *= DEFAULT_BULLETSPEED / velocity.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //_rb.constraints = RigidbodyConstraints.FreezeAll;
        Destroy(gameObject);
    }
}
