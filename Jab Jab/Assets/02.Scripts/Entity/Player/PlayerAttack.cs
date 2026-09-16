using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    private InputReader _reader;
    private Animator _animator;

    private void Awake()
    {
        _reader = this.GetComponentSafe<InputReader>();
        _animator = this.GetComponentSafe<Animator>();
    }

    private void FixedUpdate()
    {
        if(_reader.IsHeld(InputButton.Punch))
        {
            _animator.SetTrigger("Punch");
        }

        if(_reader.IsHeld(InputButton.Kick))
        {
            _animator.SetTrigger("Kick");
        }
    }
}
