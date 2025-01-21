using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControl ctrl;


    private void Awake()
    {
        ctrl = new PlayerControl();
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
