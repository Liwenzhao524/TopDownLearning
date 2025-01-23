using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    PlayerWeaponVisual _visual => GetComponentInParent<PlayerWeaponVisual>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ReloadFinish()
    {
        _visual.RecoverRigWeight();


    }

    public void WeaponGrabFinish()
    {
        _visual.SetBusyWhenGrab(false);
    }

    public void RecoverWeight()
    {
        _visual.RecoverRigWeight();
        _visual.RecoverIKWeight();
    }
}
