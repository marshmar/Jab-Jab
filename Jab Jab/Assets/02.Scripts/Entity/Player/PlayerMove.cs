using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerMove : MonoBehaviour
{
    private PlayerInput _input;
    private InputAction _moveAction;
    private Transform   _tr;
    private Rigidbody   _rigid;
    private Vector3     _moveDir;
    private float       _moveSpeed = 10.0f;

    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

    private void Awake()
    {
        _tr = this.GetComponentSafe<Transform>();
        _rigid = this.GetComponentSafe<Rigidbody>();
        _input = this.GetComponentSafe<PlayerInput>();

        InitializeMoveAction();
    }

    private void InitializeMoveAction()
    {
        _moveAction = _input.actions.FindAction("Move");

        if (_moveAction.IsNull<InputAction>()) return;

        _moveAction.performed += context =>
        {
            SetMoveDirection(context);
        };

        _moveAction.canceled += context =>
        {
            _moveDir = Vector3.zero;
        };
    }

    private void SetMoveDirection(InputAction.CallbackContext context)
    {
        Vector2 contextVec = context.ReadValue<Vector2>();
        _moveDir = new Vector3(contextVec.x, 0, contextVec.y);
    }

    private void Move()
    {
        //_rigid.MovePosition(_rigid.position + _moveDir);
        _tr.Translate(_moveDir * _moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }
}
