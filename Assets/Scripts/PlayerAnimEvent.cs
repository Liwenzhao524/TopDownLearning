using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    WeaponVisualController _visualctrl => GetComponentInParent<WeaponVisualController>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ReloadFinish()
    {
        _visualctrl.RecoverRigWeight();


    }

    public void WeaponGrabFinish()
    {
        _visualctrl.SetBusyWhenGrab(false);
    }

    public void RecoverWeight()
    {
        _visualctrl.RecoverRigWeight();
        _visualctrl.RecoverIKWeight();
    }
}
