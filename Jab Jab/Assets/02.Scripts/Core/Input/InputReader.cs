using UnityEngine;
using UnityEngine.InputSystem;
using System;

/*
 * [NOTE] InputReader는 항상 FixedUpdate가 먼저 실행되어야 하기 때문에
 *        Script Execution Order를 -99로 설정
 */
[Flags] 
public enum InputButton : ushort
{
    None = 0,
    Punch = 1 << 0,
    Kick = 1 << 1,
    Dodge = 1 << 2,
    Jump = 1 << 3,
    LockOn = 1 << 4,
    Interact = 1 << 5,
}

public struct InputFrame
{
    public InputFrame(InputButton button, Vector2 dir)
    {
        buttons = button;
        move = dir;
    }

    public InputButton buttons;
    public Vector2 move;
}

public class InputReader : MonoBehaviour
{
    private PlayerControl _control;

    private InputAction _moveAction;
    private InputAction _punchAction;
    private InputAction _lookAction;
    private InputAction _kickAction;

    // long으로 저장 시 최대 48억년 저장 가능(int의 경우 414일 저장 가능)
    private long _currentFrame;

    private InputFrame[] _inputBuffer;
    private const int BufferSize = 120;
    private InputButton _pendingButtons;
    private Vector2 _pendingMove;

    private void Awake()
    {
        _control = new PlayerControl();
        _inputBuffer = new InputFrame[BufferSize];

        BindActions();
    }

    private void BindActions()
    {
        _moveAction = _control.PlayerInputAction.Move;
        _punchAction = _control.PlayerInputAction.Punch;
        _lookAction = _control.PlayerInputAction.Look;
        _kickAction = _control.PlayerInputAction.Kick;
    }

    private void OnEnable()
    {
        _control.Enable();
    }

    private void OnDisable()
    {
        _control.Disable();
    }

    private void Update()
    {
        _pendingMove = _moveAction.ReadValue<Vector2>();
        if(_punchAction.WasPressedThisFrame())
        {
            _pendingButtons |= InputButton.Punch;
        }

        if(_kickAction.WasPressedThisFrame())
        {
            _pendingButtons |= InputButton.Kick;
        }
    }

    private void FixedUpdate()
    {
        _currentFrame++;
        var index = _currentFrame % BufferSize;
        _inputBuffer[index] = new InputFrame(_pendingButtons, _pendingMove);
        _pendingButtons = 0;
    }

    public bool WasPressedInFrame(InputButton button, int frameCount)
    {
        if (_currentFrame < (long)frameCount)
        {
            return false;   
        }

        int curr = (int)(_currentFrame % BufferSize);

        for(int i = 0; i < frameCount; i++)
        {
            var previous = (curr - i + BufferSize) % BufferSize;
            if ((_inputBuffer[previous].buttons & button) != 0)
            {
                return true;
            }
        }
        return false;
    }

    public void Consume(InputButton button, int frameCount)
    {
        if (_currentFrame < (long)frameCount)
        {
            return;
        }

        int curr = (int)(_currentFrame % BufferSize);

        for (int i = 0; i < frameCount; i++)
        {
            var previous = (curr - i + BufferSize) % BufferSize;
            if ((_inputBuffer[previous].buttons & button) != 0)
            {
                _inputBuffer[previous].buttons &= ~button;
                break;
            }
        }
    }

    public bool IsHeld(InputButton button)
    {
        int index = (int)(_currentFrame % BufferSize);
        return (_inputBuffer[index].buttons & button) != 0;
    }

    public InputFrame GetCurrent()
    {
        int index = (int)(_currentFrame % BufferSize);
        return _inputBuffer[index];
    }

    public Vector2 ReadLookRaw()
    {
        return _lookAction.ReadValue<Vector2>();
    }
}
