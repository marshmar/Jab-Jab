using UnityEngine;
using UnityEngine.InputSystem;
using Constants;

public class PlayerAttack : MonoBehaviour
{
    private InputReader _reader;
    private Animator _animator;
    private int _punchComboCount;
    private int _kickComboCount;    

    private float _timeSinceLastAttack;
    private float _attackTimer;
    private bool _isAttacking;
    private int _attackFrameThreshold = 12;

    private AttackData _currentAttackData;

    [SerializeField]
    private AttackData[] _punchCombo;
    [SerializeField]
    private AttackData[] _kickCombo;

    public bool IsAttacking
    {
        get { return _isAttacking; }
    }

    public bool CanRotateBefAttack
    {
        get { return _isAttacking && _attackTimer < 0.15f; }
    }
    private void Awake()
    {
        _reader = this.GetComponentSafe<InputReader>();
        _animator = this.GetComponentSafe<Animator>();
        _punchComboCount = 0;
        _kickComboCount = 0;
        _timeSinceLastAttack = 0.0f;
    }

    private void FixedUpdate()
    {
        if(_isAttacking)
        {
            _attackTimer += Time.fixedDeltaTime;
            if (_attackTimer > _currentAttackData.length)
            {
                _isAttacking = false;
            }
        }

        if (_reader.WasPressedInFrame(InputButton.Punch, _attackFrameThreshold))
        {
            if(!_isAttacking || _attackTimer >= _currentAttackData.length * _currentAttackData.cancelRatio)
            {
                _reader.Consume(InputButton.Punch, _attackFrameThreshold);
                Attack(_punchCombo[_punchComboCount]);
                _punchComboCount = (_punchComboCount + 1) % _punchCombo.Length;
            }
        }
        else if(_reader.WasPressedInFrame(InputButton.Kick, _attackFrameThreshold))
        {
            if (!_isAttacking || _attackTimer >= _currentAttackData.length * _currentAttackData.cancelRatio)
            {
                _reader.Consume(InputButton.Kick, _attackFrameThreshold);
                Attack(_kickCombo[_kickComboCount]);
                _kickComboCount = (_kickComboCount + 1) % _kickCombo.Length;
            }
        }
        else
        {
            _timeSinceLastAttack += Time.fixedDeltaTime;
            if (_timeSinceLastAttack >= 2.0f)
            {
                _timeSinceLastAttack = 0.0f;
                _punchComboCount = 0;
                _kickComboCount = 0;
            }
        }

    }

    private void Attack(AttackData attackData)
    {
        _currentAttackData = attackData;
        _animator.CrossFadeInFixedTime(attackData.animName, 0.06f);

        _attackTimer = 0.0f;
        _timeSinceLastAttack = 0.0f;
        _isAttacking = true;
    }
}
