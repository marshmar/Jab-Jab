using UnityEngine;
using UnityEngine.InputSystem;

public class MainCam : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    [Header("Orbit")]
    [SerializeField]
    private float _distance = 10.0f;
    [SerializeField]    
    private float _aimHeight = 1.5f;

    [Header("Look")]
    [SerializeField]
    private float _lookSpeed = 1.0f;
    [SerializeField]
    private float _minPitch = -20.0f;
    [SerializeField]
    private float _maxPitch = 80.0f;

    private Transform _tr;
    private Transform _playerTr;
    private InputAction _lookAction;

    private float _yaw;
    private float _pitch;

    private void Awake()
    {
        _tr = this.GetComponentSafe<Transform>();

        if (_player.IsNull() == false)
        {
            _playerTr = _player.transform;
            _lookAction = _player.GetComponentSafe<PlayerInput>().actions["Look"];
        }

        _pitch = 20.0f;
    }

    private void LateUpdate()
    {
        if (_playerTr.IsNull())
        {
            return;
        }

        Vector2 lookDir = _lookAction.ReadValue<Vector2>();

        _yaw += lookDir.x * _lookSpeed;
        _pitch = Mathf.Clamp(_pitch + lookDir.y * _lookSpeed, _minPitch, _maxPitch);

        Quaternion rot = Quaternion.Euler(10.0f, _yaw, 0.0f);
        Vector3 aimPoint = _playerTr.position + Vector3.up * _aimHeight;

        _tr.position = aimPoint + rot * new Vector3(0.0f, 0.0f, -_distance);
        _tr.rotation = rot;
    }
}
