using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    Player _player => GetComponent<Player>();

    PlayerControl _ctrl;

    [Header("Aim Info")]
    [SerializeField] Transform _aimPoint;
    [SerializeField] LayerMask _aimLayer;
    [SerializeField] bool _isAimPrecislyEnable = true;
    [SerializeField] bool _isAimLockEnable = true;

    [Header("Aim Line - Laser")]
    [SerializeField] LineRenderer _aimLine;

    [Header("Camera Info")]
    [SerializeField] Transform _cameraPoint;
    [Range(0.7f, 1)]
    [SerializeField] float _minCameraDis;
    [Range(1, 3f)]
    [SerializeField] float _maxCameraDis;
    [Range(4, 8)]
    [SerializeField] float _cameraSensetivity;

    Vector2 _mousePos;
    RaycastHit _lastHitInfo;

    // Start is called before the first frame update
    void Start()
    {
        AssignInputEvents();
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(_mousePos);

        if ( Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayer) )
        {
            _lastHitInfo = hitInfo;
            return hitInfo;
        }

        return _lastHitInfo;
    }

    private void AssignInputEvents()
    {
        _ctrl = _player.ctrl;

        _ctrl.Character.Aim.performed += context => _mousePos = context.ReadValue<Vector2>();
        _ctrl.Character.Aim.canceled += context => _mousePos = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        SetAimPosition();
        SetAimLine();
        SetCameraPosition();
    }

    private void SetAimLine()
    {
        float range = 5;

        Vector3 startPos = _player.weapon.GetMuzzleTransform().position;
        Vector3 endPos = startPos + _player.weapon.GetBulletDirection() * range;

        if( Physics.Raycast(startPos, _player.weapon.GetBulletDirection(), out RaycastHit hit, range) )
            endPos = hit.point;

        _aimLine.SetPosition(0, startPos);
        _aimLine.SetPosition(1, 0.2f * startPos + 0.8f * endPos);
        _aimLine.SetPosition(2, endPos);
    }

    private void SetAimPosition()
    {
        Transform target = GetLockTargetTransform();
        if( target != null && _isAimLockEnable) 
        {
            _aimPoint.position = target.GetComponent<Renderer>() != null ? target.GetComponent<Renderer>().bounds.center : target.position;
            return;
        }

        _aimPoint.position = GetMouseHitInfo().point;
        _aimPoint.position += _isAimPrecislyEnable ? Vector3.up : Vector3.zero;
    }
    private void SetCameraPosition() =>  _cameraPoint.position = Vector3.Lerp(_cameraPoint.position, GetCameraCenter(), _cameraSensetivity * Time.deltaTime);

    private Vector3 GetCameraCenter()
    {
        float maxCameraDis = _player.movement.moveInput.y < -0.5f ? _minCameraDis : _maxCameraDis;

        Vector3 aimDirection = (GetMouseHitInfo().point - transform.position).normalized;
        float distance = Vector3.Distance(GetMouseHitInfo().point, transform.position);
        distance = Mathf.Clamp(distance, _minCameraDis, maxCameraDis);
        aimDirection *= distance;
        Vector3 target = transform.position + aimDirection + Vector3.up;
        return target;
    }

    public Transform GetLockTargetTransform()
    {
        Transform target = null;
        if ( GetMouseHitInfo().transform.GetComponent<LockTarget>() != null )
            target = GetMouseHitInfo().transform;

        return target;
    }

    public bool CanAimPrecisly() => _isAimPrecislyEnable;
    public Transform GetAimPoint() => _aimPoint;

}
