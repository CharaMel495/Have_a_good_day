using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// åªç›íNÇ…ì¸óÕèÓïÒÇìnÇ∑Ç©
/// </summary>
public enum InputHandler
{
    Player,
    UI
}

/// <summary>
/// ì¸óÕÇ…ä÷åWÇ∑ÇÈãLèqÇÕÇ∑Ç◊ÇƒÇ±Ç±Ç…
/// </summary>
public class InputSystemManager : SingletonMonoBehaviour<InputSystemManager>
{
    [SerializeField] 
    private InputActionAsset _inputActions;

    // ActionMaps
    private InputActionMap _playerMap;
    private InputActionMap _uiMap;

    // InputActions
    private InputAction _moveAction;
    private InputAction _shootAction;
    private InputAction _dashAction;
    private InputAction _vacuumAction;
    private InputAction _slowAction;
    private InputAction _pauseAction;
    private InputAction _rightTrigger;
    private InputAction _leftTrigger;
    private InputAction _rightGrip;
    private InputAction _leftGrip;
    private InputAction _openUIAction;

    [SerializeField] 
    private float _stickDeadzoneSqrt = 0.1f;

    private Timer _timer;
    private static Vector2 _inputDir;
    private static Direction _dirPreview = Direction.None;

    public static InputHandler CurrentHandler { get; private set; } = InputHandler.Player;
    private static InputHandler _remainingHandler;

    private static Dictionary<string, List<Action>> _bindedActions;

    public async Task Initialize()
    {
        _timer = new();
        _remainingHandler = CurrentHandler;

        _playerMap = _inputActions.FindActionMap("Player");
        _uiMap = _inputActions.FindActionMap("UI");

        _moveAction = _playerMap.FindAction("Move");
        _shootAction = _playerMap.FindAction("Shoot");
        _dashAction = _playerMap.FindAction("Dash");
        _vacuumAction = _playerMap.FindAction("Vacuum");
        _slowAction = _playerMap.FindAction("Slow");
        _pauseAction = _playerMap.FindAction("Pause");

        _openUIAction = _playerMap.FindAction("OpenUI");

        _rightTrigger = Instance._playerMap.FindAction("RightTrigger");
        _leftTrigger = Instance._playerMap.FindAction("LeftTrigger");
        _rightGrip = Instance._playerMap.FindAction("RightGrip");
        _leftGrip = Instance._playerMap.FindAction("LeftGrip");

        _playerMap.Enable();
        _uiMap.Disable();

        _bindedActions = new();
    }

    private void Update()
    {
        _timer?.Update();

        CheckBindedAction();
    }

    public static void BindAction(string bindInputActionName, Action func)
    {
        if (_bindedActions.ContainsKey(bindInputActionName))
            _bindedActions[bindInputActionName].Add(func);
        else
            _bindedActions.Add(bindInputActionName, new List<Action> { func });
    }

    private void CheckBindedAction()
    {
        foreach (var item in _bindedActions)
        {
            if (CheckActionPressed(item.Key, CurrentHandler))
                foreach (var func in item.Value)
                    func.Invoke();
        }
    }

    public static Direction CheckInputDirection(InputHandler handle, bool isPrevious = false)
    {
        if (CurrentHandler != handle || (handle == InputHandler.Player && GameManager.IsFade))
            return Direction.None;

        Vector2 input = Instance._moveAction.ReadValue<Vector2>();

        Direction GetDirection()
        {
            return (Mathf.Abs(input.x), Mathf.Abs(input.y)) switch
            {
                ( < 0.5f, < 0.5f) => Direction.None,
                ( > 0, < 0.5f) => input.x > 0 ? Direction.Right : Direction.Left,
                ( < 0.5f, > 0) => input.y > 0 ? Direction.Up : Direction.Down,
                _ => input.x > 0
                    ? input.y > 0 ? Direction.UpRight : Direction.DownRight
                    : input.y > 0 ? Direction.UpLeft : Direction.DownLeft,
            };
        }

        var dir = GetDirection();

        if (isPrevious && _dirPreview == dir)
            return Direction.None;

        _dirPreview = dir;
        return dir;
    }

    public static Vector2 GetInputDirection(InputHandler handle)
    {
        if (CurrentHandler != handle)
            return Vector2.zero;

        return Instance._moveAction.ReadValue<Vector2>();
    }

    public static bool CheckActionPressed(string actionName, InputHandler handle, bool isPrevious = false)
    {
        if (CurrentHandler != handle || (handle == InputHandler.Player && GameManager.IsFade))
            return false;

        if (Instance._playerMap == null)
        {
            Debug.LogWarning("_playerMap is Null");
            return false;
        }

        var action = Instance._playerMap.FindAction(actionName);
        if (action == null) return false;

        return isPrevious ? action.IsPressed() : action.WasPressedThisFrame();
    }

    public static bool IsTriggerPressed(bool rightHand = true, bool isGrip = false, InputHandler handle = InputHandler.Player, bool isPrevious = false)
    {
        if (CurrentHandler != handle || (handle == InputHandler.Player && GameManager.IsFade))
            return false;

        var  action = (rightHand, isGrip) switch
        {
            (true, false) => Instance._rightTrigger,
            (true, true) => Instance._rightGrip,
            (false, false) => Instance._leftTrigger,
            (false, true) => Instance._leftGrip
        };

        
        if (action == null)
        {
            Debug.LogWarning($"This action was't binded.");
            return false;
        }

        return isPrevious ? action.IsPressed() : action.WasPressedThisFrame();
    }


    public static bool IsActionPressed(InputHandler handle, string actionName)
    {
        return CheckActionPressed(actionName, handle, isPrevious: true);
    }

    public void ChangeInputHandler(InputHandler nextHandler)
    {
        _remainingHandler = CurrentHandler;
        _timer.CreateTask(() => SwapNextHandle(nextHandler), GameManager.FRAME);
    }

    public void SwapNextHandle(InputHandler nextHandler)
    {
        if (nextHandler == CurrentHandler) 
            return;

        if (nextHandler == InputHandler.Player)
        {
            _uiMap.Disable();
            _playerMap.Enable();
        }
        else
        {
            _playerMap.Disable();
            _uiMap.Enable();
        }

        CurrentHandler = nextHandler;
    }

    public void ReturnHandle()
    {
        SwapNextHandle(_remainingHandler);
    }
}
