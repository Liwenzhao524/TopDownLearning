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

    [Header("Camera Info")]
    [SerializeField] Transform _cameraPoint;
    [Range(0.7f, 1)]
    [SerializeField] float _minCameraDis;
    [Range(1, 3f)]
    [SerializeField] float _maxCameraDis;
    [Range(4, 8)]
    [SerializeField] float _cameraSensetivity;
    Vector2 _aimInput;
    RaycastHit _lastHitInfo;

    // Start is called before the first frame update
    void Start()
    {
        AssignInputEvents();
    }

    public RaycastHit GetMouseHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(_aimInput);

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

        _ctrl.Character.Aim.performed += context => _aimInput = context.ReadValue<Vector2>();
        _ctrl.Character.Aim.canceled += context => _aimInput = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        _aimPoint.position = GetMouseHit().point + Vector3.up;
        _cameraPoint.position = Vector3.Lerp(_cameraPoint.position, GetCameraCenter(), _cameraSensetivity * Time.deltaTime);
    }

    private Vector3 GetCameraCenter()
    {
        float maxCameraDis = _player.movement.moveInput.y < -0.5f ? _minCameraDis : _maxCameraDis;

        Vector3 aimDirection = (GetMouseHit().point - transform.position).normalized;
        float distance = Vector3.Distance(GetMouseHit().point, transform.position);
        distance = Mathf.Clamp(distance, _minCameraDis, maxCameraDis);
        aimDirection *= distance;
        Vector3 target = transform.position + aimDirection + Vector3.up;
        return target;
    }
}
