using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerControl _ctrl;
    CharacterController _characterController => GetComponent<CharacterController>();
    Animator _animator => GetComponentInChildren<Animator>();
    Player _player => GetComponent<Player>();

    [SerializeField] float _walkSpeed;
    [SerializeField] float _runSpeed;
    [SerializeField] float _turnSpeed;

    float _currentSpeed;
    public Vector2 moveInput;
    Vector3 _moveDirection;
    float _verticalVelocity;
    bool _isWalk;
    bool _isRun;

    private void Start()
    {
        _currentSpeed = _walkSpeed;
        AssignInputEvents();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyRotation();
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

    private void ApplyRotation()
    {
        Vector3 lookDirection = _player.aim.GetMouseHit().point - transform.position;
        lookDirection.y = 0;
        lookDirection.Normalize();

        Quaternion target = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, target, _turnSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        _moveDirection = new(moveInput.x, 0, moveInput.y);
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
            moveInput = context.ReadValue<Vector2>();
            _isWalk = true;
        };

        _ctrl.Character.Movement.canceled += context =>
        {
            moveInput = Vector2.zero;
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


    }

}
