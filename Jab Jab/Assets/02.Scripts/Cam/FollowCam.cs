using UnityEngine;
using UnityEngine.InputSystem;

// TODO: 인풋시스템 중복 적용 되는지 확인 필요, 카메라 회전 구현 필요
public class FollowCam : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;
    private Transform _tr;

    // target
    [SerializeField] private Transform _target;
    [SerializeField] private float _height = 20.0f;
    [SerializeField] private float _damping = 10.0f;
    [SerializeField, Range(1.0f, 20.0f)] private float _distance = 1.0f;

    // Mouse
    [SerializeField] private PlayerInput _input;
    [SerializeField] private InputAction _rotateAction;

    private void Awake()
    {
        _input = this.GetComponentSafe<PlayerInput>();

        InitializeRotateAction();
    }


    private void Start()
    {
        _tr = this.GetComponentSafe<Transform>();   
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void InitializeRotateAction()
    {
        _rotateAction = _input.actions.FindAction("Rotate");

        if (_rotateAction.IsNull<InputAction>()) return;

        _rotateAction.performed += context =>
        {
            Rotate(context);
        };
    }

    private void FollowTarget()
    {
        if (_target == null) return;

        Vector3 pos = _target.position + new Vector3(0, _height, -_distance);
        _tr.position = Vector3.SmoothDamp(_tr.position, pos, ref _velocity, _damping);
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        Debug.Log("Rotate");
    }
}
