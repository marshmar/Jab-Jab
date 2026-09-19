using UnityEngine;
using Constants;
using System.Collections.Generic;

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

    private List<IHittable> _damagedHittables;

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
        _damagedHittables = new List<IHittable>();
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

            if (_attackTimer >= _currentAttackData.BusyDuration * _currentAttackData.HitStartRatio
                && _attackTimer <= _currentAttackData.BusyDuration * _currentAttackData.HitEndRatio)
            {
                Damage();
            }
        }



        if (!_isAttacking && _currentAttackData != _idleData)
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
                if(_reader.WasPressedWithIn(_currentAttackData.ComboLinks[i].InputButton, _inputBufferFrames))
                {
                    _reader.Consume(_currentAttackData.ComboLinks[i].InputButton, _inputBufferFrames);
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

        _damagedHittables.Clear();
    }

    private void Damage()
    {

        Vector3 center = transform.TransformPoint(_currentAttackData.HitBoxOffset);
        Collider[] enemies = Physics.OverlapBox(center, _currentAttackData.HitBoxSize * 0.5f,
            transform.rotation, LayerConstants.EnemyLayerMask);

        
        foreach(Collider enemy in enemies)
        {
            IHittable hittable = enemy.GetComponentInParent<IHittable>();

            if (hittable == null || _damagedHittables.Contains(hittable))
            {
                continue;
            }

            Vector3 myTrans = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 targetTrans = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
            Vector3 dir = (targetTrans - myTrans).normalized;
            HitData hitData = new HitData(10.0f, dir);
            hittable.TakeHit(hitData);
            _damagedHittables.Add(hittable);

        }
    }

    private void OnDrawGizmos()
    {
        if (_currentAttackData == null)
            return;

        bool isHitActive = _isAttacking
            && _attackTimer >= _currentAttackData.BusyDuration * _currentAttackData.HitStartRatio
            && _attackTimer <= _currentAttackData.BusyDuration * _currentAttackData.HitEndRatio;

        // 이 다음부터 그리는건 전부 캐릭터 로컬 좌표로 해석해라
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = isHitActive ? Color.red : Color.green;
        Gizmos.DrawWireCube(_currentAttackData.HitBoxOffset, _currentAttackData.HitBoxSize);
    }
}
