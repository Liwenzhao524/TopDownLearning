using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerControl _ctrl;
    CharacterController _characterController => GetComponent<CharacterController>();
    Animator _animator => GetComponentInChildren<Animator>();
    Player _player => GetComponent<Player>();

    [Header("Movement Info")]
    [SerializeField] float _walkSpeed;
    [SerializeField] float _runSpeed;
    float _currentSpeed;
    Vector3 _moveDirection;
    float _verticalVelocity;
    bool _isWalk;
    bool _isRun;

    [Header("Aim Info")]
    [SerializeField] Transform _aimPoint;
    [SerializeField] LayerMask _aimLayer;
    Vector3 _lookDirection;

    Vector2 _moveInput;
    Vector2 _aimInput;


    private void Start()
    {
        _currentSpeed = _walkSpeed;
        AssignInputEvents();
    }

    private void Update()
    {
        ApplyMovement();
        Aim();
        AnimatorControl();

    }

    private void AnimatorControl()
    {
        float velocityX = Vector3.Dot(_moveDirection.normalized, transform.right);
        float velocityZ = Vector3.Dot(_moveDirection.normalized, transform.forward);

        _animator.SetBool("IsWalk", _isWalk);
        _animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
        _animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        _animator.SetBool("IsRun", _isRun);
    }

    private void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(_aimInput);

        if ( Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayer) )
        {
            _lookDirection = hitInfo.point - transform.position;
            _lookDirection.y = 0;
            _lookDirection.Normalize();

            transform.forward = _lookDirection;
            _aimPoint.position = new Vector3(hitInfo.point.x, transform.position.y + 1, hitInfo.point.z);
        }
    }

    private void ApplyMovement()
    {
        _moveDirection = new(_moveInput.x, 0, _moveInput.y);
        ApplyGravity();

        if ( _moveDirection.magnitude > 0 )
        {
            _characterController.Move(_moveDirection * Time.deltaTime * _currentSpeed);
        }
    }
    private void ApplyGravity()
    {
        if ( !_characterController.isGrounded)
        {
            _verticalVelocity -= 9.81f * Time.deltaTime;
            _moveDirection.y = _verticalVelocity;
        }
        else
        {
            _verticalVelocity = -0.5f;
        }
    }

    private void AssignInputEvents()
    {
        _ctrl = _player.ctrl;

        _ctrl.Character.Movement.performed += context =>
        {
            _moveInput = context.ReadValue<Vector2>();
            _isWalk = true;
        };

        _ctrl.Character.Movement.canceled += context =>
        {
            _moveInput = Vector2.zero;
            _isWalk = false;
        };

        _ctrl.Character.Run.performed += context =>
        {
            _isRun = true;
            _currentSpeed = _runSpeed;
        };

        _ctrl.Character.Run.canceled += context =>
        {
            _isRun = false;
            _currentSpeed = _walkSpeed;
        };

        _ctrl.Character.Aim.performed += context => _aimInput = context.ReadValue<Vector2>();
        _ctrl.Character.Aim.canceled += context => _aimInput = Vector2.zero;
    }

}
