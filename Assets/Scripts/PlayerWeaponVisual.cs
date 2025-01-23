using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum GrabType { BackGrab, SideGrab};

public class PlayerWeaponVisual : MonoBehaviour
{
    [SerializeField] Transform[] _weapons;

    Transform _currentWeapon;

    Animator _animator => GetComponentInChildren<Animator>();

    [Header("Left Hand IK")]
    [SerializeField] Transform _leftHandIKTarget;
    [SerializeField] TwoBoneIKConstraint _leftHandConstraint;
    bool _isIKConstraintRecover;

    Rig _rig => GetComponentInChildren<Rig>();
    bool _isRigWeightRecover;

    private void Start()
    {
        SwitchWeapon(_weapons[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(_weapons[0]);
            SwitchAnimationLayer(1);
            GrabAnimation(GrabType.SideGrab);
        }
        if ( Input.GetKeyDown(KeyCode.Alpha2) )
        {
            SwitchWeapon(_weapons[1]);
            SwitchAnimationLayer(1);
            GrabAnimation(GrabType.SideGrab);
        }
        if ( Input.GetKeyDown(KeyCode.Alpha3) )
        {
            SwitchWeapon(_weapons[2]);
            SwitchAnimationLayer(2);
            GrabAnimation(GrabType.BackGrab);
        }

        if ( Input.GetKeyDown(KeyCode.R) )
        {
            _animator.SetTrigger("Reload");
            _rig.weight = 0;
        }

        if(_isRigWeightRecover)
        {
            _rig.weight += 3 * Time.deltaTime;
            if(_rig.weight >= 1) _isRigWeightRecover = false;
        }

        if ( _isIKConstraintRecover )
        {
            _leftHandConstraint.weight += 3 * Time.deltaTime;
            if( _leftHandConstraint.weight >= 1) _isIKConstraintRecover = false;
        }
    }

    public void RecoverRigWeight() => _isRigWeightRecover = true;

    public void RecoverIKWeight() => _isIKConstraintRecover = true;

    void GrabAnimation(GrabType grabType)
    {
        _leftHandConstraint.weight = 0;
        _rig.weight = 0.15f;
        _animator.SetFloat("GrabType", (float)grabType);
        _animator.SetTrigger("Grab");
        SetBusyWhenGrab(true);
    }

    public void SetBusyWhenGrab(bool isBusy)
    {
        _animator.SetBool("IsBusy", isBusy);
    }


    void SwitchWeapon(Transform weapon = null)
    {
        for ( int i = 0; i < _weapons.Length; i++ )
        {
            _weapons[i].gameObject.SetActive(false);
        }

        if ( weapon == null ) return;

        weapon.gameObject.SetActive(true);
        _currentWeapon = weapon;

        SetLeftHandIK();
    }

    void SetLeftHandIK()
    {
        Transform target = _currentWeapon.GetComponentInChildren<LeftHandIKManage>().transform;

        _leftHandIKTarget.localPosition = target.localPosition;
        _leftHandIKTarget.localRotation = target.localRotation;
    }

    void SwitchAnimationLayer(int layer)
    {
        for(int i = 1; i < _animator.layerCount; i++ )
        {
            _animator.SetLayerWeight(i, 0);
        }

        _animator.SetLayerWeight(layer, 1);
    }

}
