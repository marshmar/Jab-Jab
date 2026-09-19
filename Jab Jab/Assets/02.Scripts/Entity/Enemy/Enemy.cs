using UnityEngine;

public class Enemy : MonoBehaviour, IHittable
{
    private bool _isStaggering;
    private float _staggerTimer;
    private float _currentStaggerDuration;

    private Animator _animator;
    private HitStop _hitStop;

    [SerializeField]
    private HitReaction[] _hitReactions;

    public bool IsStaggering {
        get => _isStaggering;
    }

    private void Awake()
    {
        _isStaggering = false;
        _staggerTimer = 0.0f;

        _animator = GetComponent<Animator>();
        _hitStop = GetComponent<HitStop>();

        _currentStaggerDuration = 0.0f;
    }

    private void FixedUpdate()
    {
        if(IsStaggering && !_hitStop.IsFrozen)
        {
            _staggerTimer += Time.fixedDeltaTime;
            if(_staggerTimer >= _currentStaggerDuration)
            {
                _isStaggering = false;
                _animator.CrossFadeInFixedTime("Idle", 0.06f);
            }
        }
    }

    private void PlayHitReaction(HitStrength hitStrength)
    {
        int staggerIndex = (int)hitStrength;
        if (staggerIndex < _hitReactions.Length)
        {
            _staggerTimer = 0.0f;
            _isStaggering = true;
            _animator.CrossFadeInFixedTime(_hitReactions[staggerIndex].AnimStateName, 0.0f);
            _currentStaggerDuration = _hitReactions[staggerIndex].Duration;
        }
    }
    public void TakeHit(HitData hitData)
    {
        Debug.Log($"Take Hit! Damage: {hitData.Damage}, HitStrength: {hitData.HitStrength}");
        PlayHitReaction(hitData.HitStrength);
        _hitStop.Freeze(hitData.HitStopFrames);
    }
}
