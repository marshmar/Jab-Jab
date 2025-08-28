using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private Vector3 _velocity = Vector3.zero;
    private Transform _tr;

    // target
    [SerializeField] private Transform _target;
    [SerializeField] private float _height = 20.0f;
    [SerializeField] private float _damping = 10.0f;
    [SerializeField, Range(1.0f, 20.0f)] private float _distance = 1.0f;

    private void Start()
    {
        _tr = this.GetComponentSafe<Transform>();   
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (_target == null) return;

        Vector3 pos = _target.position + new Vector3(0, _height, -_distance);
        _tr.position = Vector3.SmoothDamp(_tr.position, pos, ref _velocity, _damping);

        Debug.Log("Follow Target");
    }
}
