using UnityEngine;
using UnityEngine.InputSystem;
using Constants;

public class PlayerAttack : MonoBehaviour
{
    private InputReader _reader;
    private Animator _animator;   

    private float _comboResetTimer;
    private float _attackTimer;
    private bool _isAttacking;
    private int _inputBufferFrames = 12;

    private AttackData _currentAttackData;

    [SerializeField]
    private AttackData _idleData;

    public bool IsAttacking
    {
        get { return _isAttacking; }
    }

    public bool IsInAimWindow
    {
        get { return _isAttacking && _attackTimer < 0.15f; }
    }

    private void Awake()
    {
        _reader = this.GetComponentSafe<InputReader>();
        _animator = this.GetComponentSafe<Animator>();
        _comboResetTimer = 0.0f;
        _currentAttackData = _idleData;
    }

    private void FixedUpdate()
    {
        if(_isAttacking)
        {
            _attackTimer += Time.fixedDeltaTime;
            if (_attackTimer > _currentAttackData.BusyDuration)
            {
                _isAttacking = false;
            }
        }
        
        if(!_isAttacking && _currentAttackData != _idleData)
        {
            _comboResetTimer += Time.fixedDeltaTime;
            if (_comboResetTimer >= 2.0f)
            {
                _comboResetTimer = 0.0f;
                _currentAttackData = _idleData;
            }
        }

        if (!_isAttacking || _attackTimer >= _currentAttackData.BusyDuration * _currentAttackData.CancelStartRatio)
        {
            for (int i = 0; i < _currentAttackData.ComboLinks.Length; i++)
            {
                if(_reader.WasPressedWithIn(_currentAttackData.ComboLinks[i].Button, _inputBufferFrames))
                {
                    _reader.Consume(_currentAttackData.ComboLinks[i].Button, _inputBufferFrames);
                    Attack(_currentAttackData.ComboLinks[i].Next);
                    break;
                }
            }
        }


    }

    private void Attack(AttackData attackData)
    {
        _currentAttackData = attackData;
        _animator.CrossFadeInFixedTime(attackData.AnimStateName, 0.06f);

        _attackTimer = 0.0f;
        _comboResetTimer = 0.0f;
        _isAttacking = true;
    }
}
