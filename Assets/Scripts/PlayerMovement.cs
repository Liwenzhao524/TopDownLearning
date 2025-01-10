using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerControl _control;

    public Vector2 moveInput;
    public Vector2 aimInput;

    private void Awake()
    {
        _control = new();

        _control.Character.Fire.performed += context => Shoot();

        _control.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        _control.Character.Movement.canceled += context => moveInput = Vector2.zero;

        _control.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        _control.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }

    private void Shoot()
    {
        Debug.Log("Shoot");
    }

    private void OnEnable()
    {
        _control.Enable();
    }

    private void OnDisable()
    {
        _control.Disable();
    }
}
