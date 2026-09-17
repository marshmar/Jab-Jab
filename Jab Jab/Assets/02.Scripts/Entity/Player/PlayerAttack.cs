using UnityEngine;
using UnityEngine.InputSystem;
using Constants;

public class PlayerAttack : MonoBehaviour
{
    private InputReader _reader;
    private Animator _animator;
    private int _comboCount;
    private float _timeSinceLastAttack;
    private string[] _attacksInPlace;
    private string[] _attacksForward;
    private float _attackTimer;
    private bool _isAttacking;
    private int _attackFrameThreshold = 12;

    // 지금 재생 중인 공격의 길이. _comboCount는 이미 다음 타를 가리키므로 따로 들고 있어야 한다
    private float _currentAttackLength;

    private float[] _attackInPlaceAnimLengths;
    private float[] _attackForwardAnimLengths;

    [SerializeField]
    private float _nextAttackThreshold = 0.6f;

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
        _comboCount = 0;
        _timeSinceLastAttack = 0.0f;

        _attacksInPlace = new string[3] { 
            AnimConstants.JabPunchInPlaceAnimHash,
            AnimConstants.StraightPunchInPlaceAnimHash,
            AnimConstants.HookPunchInPlaceAnimHash
        };

        _attacksForward = new string[3] {
            AnimConstants.JabPunchForwardAnimHash,
            AnimConstants.StraightPunchForwardAnimHash,
            AnimConstants.HookPunchForwardAnimHash
        };

        _attackInPlaceAnimLengths = new float[3]
        {
            0.5f,
            0.5f,
            0.5f
        };

        _attackForwardAnimLengths = new float[3]
        {
            0.5f,
            0.5f,
            0.5f
        };

    }

    private void FixedUpdate()
    {
        if(_isAttacking)
        {
            _attackTimer += Time.fixedDeltaTime;
            if (_attackTimer > _currentAttackLength)
            {
                _isAttacking = false;
            }
        }

        if (_reader.WasPressedInFrame(InputButton.Punch, _attackFrameThreshold))
        {
            if(!_isAttacking || _attackTimer >= _currentAttackLength * _nextAttackThreshold)
            {
                _reader.Consume(InputButton.Punch, _attackFrameThreshold);
                Attack();
            }
        }
        else
        {
            _timeSinceLastAttack += Time.fixedDeltaTime;
            if(_timeSinceLastAttack >= 2.0f)
            {
                _timeSinceLastAttack = 0.0f;
                _comboCount = 0;
            }
        }

    }

    private void Attack()
    {
        // _comboCount를 올리기 전에 이번에 쓸 클립과 길이를 먼저 가져온다
        _currentAttackLength = _attackInPlaceAnimLengths[_comboCount];
        _animator.CrossFadeInFixedTime(_attacksInPlace[_comboCount], 0.06f);
        _comboCount = (_comboCount + 1) % 3;

        _attackTimer = 0.0f;
        _timeSinceLastAttack = 0.0f;
        _isAttacking = true;
    }
}
