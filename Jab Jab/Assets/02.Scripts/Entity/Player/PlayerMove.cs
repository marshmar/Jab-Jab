using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Transform _camTr;

    private InputReader _reader;
    private Rigidbody _rigid;
    private Animator _animator;
    private Transform _tr;
    private PlayerAttack _playerAttack;
    private Vector2 _moveDir;

    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _moveRotateSpeed = 10.0f;
    [SerializeField]
    private float _attackRotateSpeed = 40.0f;

    private void Awake()
    {
        _reader = this.GetComponentSafe<InputReader>();
        _rigid = this.GetComponentSafe<Rigidbody>();
        _animator = this.GetComponentSafe<Animator>();
        _tr = this.GetComponentSafe<Transform>();
        _playerAttack = this.GetComponentSafe<PlayerAttack>();

        _moveDir = Vector2.zero;
        _moveSpeed = 5.0f;

        _camTr.IsNull();
    }

    private void FixedUpdate()
    {
        _moveDir = _reader.GetCurrent().move;
        if (_moveDir != Vector2.zero)
        {
            Vector3 camForward = new Vector3(_camTr.forward.x, 0, _camTr.forward.z);
            Vector3 camRight = new Vector3(_camTr.right.x, 0, _camTr.right.z);
            Vector3 moveInput = (camForward * (_moveDir.y) + camRight * (_moveDir.x)).normalized;

            if(_playerAttack.IsAttacking)
            {
                if(_playerAttack.IsInAimWindow)
                {
                    Rotate(moveInput, _attackRotateSpeed);
                }
            }
            else
            {
                Move(moveInput);
                Rotate(moveInput, _moveRotateSpeed);
            }


        }
        _animator.SetBool("isMoving", _moveDir != Vector2.zero);

    }


    private void Move(Vector3 moveInput)
    {
        _rigid.MovePosition(_rigid.position + moveInput * _moveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate(Vector3 moveInput, float speed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveInput);
        _tr.rotation = Quaternion.Slerp(_tr.rotation, targetRotation, Time.fixedDeltaTime * speed);
    }
}
