using UnityEngine;

public class HitStop : MonoBehaviour
{
    private Animator _animator;
    private bool _isFrozen;
    private int _frozenFrameCounts;
    private int _hitStopFrames;
    public bool IsFrozen { 
        get => _isFrozen; 
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(_isFrozen)
        {
            if(++_frozenFrameCounts >= _hitStopFrames)
            {
                _frozenFrameCounts = 0;
                _isFrozen = false;
                _animator.speed = 1;
            }
        }
    }

    public void Freeze(int frames)
    {
        _animator.speed = 0;
        _isFrozen = true;
        _frozenFrameCounts = 0;
        _hitStopFrames = frames;
    }
}
