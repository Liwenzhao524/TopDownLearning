using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] Transform[] _weapons;


    private void Start()
    {
        SwitchOffWeapons();
    }

    private void Update()
    {
        // SwitchWeapon With 1234
    }

    void SwichOnWeapon(Transform weapon)
    {
        SwitchOffWeapons();
        weapon.gameObject.SetActive(true);
    }

    void SwitchOffWeapons()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].gameObject.SetActive(false);
        }
    }
}
