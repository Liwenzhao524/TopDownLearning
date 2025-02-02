using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody _rb => GetComponent<Rigidbody>();

    private void OnCollisionEnter(Collision collision)
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
