using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Transform _camTr;

    private PlayerInput _input;
    private Rigidbody _rigid;
    private Animator _animator;
    private Transform _tr;


    private Vector2 _moveDir;

    [SerializeField]
    private float _moveSpeed;

    private void Awake()
    {
        _input = this.GetComponentSafe<PlayerInput>();
        _rigid = this.GetComponentSafe<Rigidbody>();
        _animator = this.GetComponentSafe<Animator>();
        _tr = this.GetComponentSafe<Transform>();   

        _moveDir = Vector2.zero;
        _moveSpeed = 5.0f;

        _input.actions["Move"].performed += context =>
        {
            _moveDir = context.ReadValue<Vector2>();
        };

        _input.actions["Move"].canceled += context =>
        {
            _moveDir = Vector2.zero;
        };

        _input.actions["Dodge"].performed += context =>
        {
            _animator.SetTrigger("Dodge");
        };

        _camTr.IsNull();
    }

    private void FixedUpdate()
    {
        if(_moveDir != Vector2.zero)
        {
            Vector3 camForward = new Vector3(_camTr.forward.x, 0, _camTr.forward.z);
            Vector3 camRight = new Vector3(_camTr.right.x, 0, _camTr.right.z);
            Vector3 move = (camForward * (_moveDir.y) + camRight * (_moveDir.x)).normalized;

            Move(move);
            Rotate(move);
        }
        _animator.SetBool("isMoving", _moveDir != Vector2.zero);
    }

    private void Move(Vector3 move)
    {
        _rigid.MovePosition(_rigid.position + move * _moveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate(Vector3 move)
    {
        Quaternion targetRotation = Quaternion.LookRotation(move);
        _tr.rotation = Quaternion.Slerp(_tr.rotation, targetRotation, Time.fixedDeltaTime * 10f);
    }
}
