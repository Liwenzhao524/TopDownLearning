using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] Transform[] _weapons;

    Transform _currentWeapon;

    [SerializeField] Transform _leftHandIKTarget;

    private void Start()
    {
        SwitchOnWeapon(_weapons[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchOnWeapon(_weapons[0]);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchOnWeapon(_weapons[1]);
    }

    void SwitchOnWeapon(Transform weapon)
    {
        SwitchOffWeapons();
        weapon.gameObject.SetActive(true);
        _currentWeapon = weapon;

        SetLeftHandIK();
    }

    void SwitchOffWeapons()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].gameObject.SetActive(false);
        }
    }

    void SetLeftHandIK()
    {
        Transform target = _currentWeapon.GetComponentInChildren<LeftHandIKManage>().transform;

        _leftHandIKTarget.localPosition = target.localPosition;
        _leftHandIKTarget.localRotation = target.localRotation;
    }
}
