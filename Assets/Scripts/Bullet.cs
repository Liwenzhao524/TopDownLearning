using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody _rb => GetComponent<Rigidbody>();

    [SerializeField] GameObject _bulletImpactFX;

    const float DEFAULT_BULLETSPEED = 20;

    public void SetupBullet(Vector3 velocity)
    {
        _rb.velocity = velocity;
        _rb.mass *= DEFAULT_BULLETSPEED / velocity.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);
        Destroy(gameObject);
    }

    private void CreateImpactFX(Collision collision)
    {
        if(collision.contacts.Length > 0 )
        {
            ContactPoint contact = collision.contacts[0];

            GameObject impactFX = Instantiate(_bulletImpactFX, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(impactFX, 1);
        }
    }
}
