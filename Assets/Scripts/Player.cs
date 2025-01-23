using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControl ctrl { get; private set; }
    public PlayerAim aim { get; private set; }
    public PlayerMovement movement { get; private set; }   

    private void Awake()
    {
        ctrl = new PlayerControl();

        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        ctrl.Enable();
    }
    private void OnDisable()
    {
        ctrl.Disable();
    }
}
