using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    PlayerControl _ctrl;
    Player _player => GetComponent<Player>();

    Animator _animator => GetComponentInChildren<Animator>();

    private void Start()
    {
        AssignInputEvents();

    }

    void Shoot()
    {
        _animator.SetTrigger("Fire");

    }

    private void AssignInputEvents()
    {
        _ctrl = _player.ctrl;

        _ctrl.Character.Fire.performed += context => Shoot();
    }

}
