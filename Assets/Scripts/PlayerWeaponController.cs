using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    PlayerControl _ctrl;
    Player _player => GetComponent<Player>();

    Animator _animator => GetComponentInChildren<Animator>();


    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] float _bulletSpeed;
    [SerializeField] Transform _muzzle;

    [SerializeField] Transform _weaponHolder;
    Transform _aimPoint => _player.aim.GetAimPoint();
 
    private void Start()
    {
        AssignInputEvents();
    }

    void Shoot()
    {
        GameObject newBullet = Instantiate(_bulletPrefab, _muzzle.position, Quaternion.LookRotation(_muzzle.forward));
        newBullet.GetComponent<Bullet>().SetupBullet(GetBulletDirection() * _bulletSpeed);
        //newBullet.GetComponent<Rigidbody>().velocity = GetBulletDirection() * _bulletSpeed;
        Destroy(newBullet, 5);

        _animator.SetTrigger("Fire");
    }

    public Vector3 GetBulletDirection()
    {
        Vector3 dir = _aimPoint.position - _muzzle.position;
        
        _weaponHolder.LookAt(_aimPoint);
        _muzzle.LookAt(_aimPoint);
        
        if(!_player.aim.CanAimPrecisly() && _player.aim.GetLockTargetTransform() == null)
            dir.y = 0; 
        dir = dir.normalized;

        return dir;
    }

    public Transform GetMuzzleTransform() => _muzzle;

    private void AssignInputEvents()
    {
        _ctrl = _player.ctrl;

        _ctrl.Character.Fire.performed += context => Shoot();
    }

}
