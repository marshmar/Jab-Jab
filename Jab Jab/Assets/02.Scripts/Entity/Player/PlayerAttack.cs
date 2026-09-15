using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    private PlayerInput _input;
    private Animator _animator;

    private void Awake()
    {
        _input = this.GetComponentSafe<PlayerInput>();
        _animator = this.GetComponentSafe<Animator>();

        _input.actions["Attack"].performed += context =>
        {
            Attack();
        };

    }


    private void Attack()
    {
        _animator.SetTrigger("Attack");
    }
}
