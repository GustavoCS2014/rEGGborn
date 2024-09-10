using System;
using Reggborn.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reggborn.Inputs
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        public static event Action<InputAction.CallbackContext> OnAnyInput;
        public static event Action<InputAction.CallbackContext> OnLayEgg;
        public static event Action<InputAction.CallbackContext, Vector2> OnMovePad;

        public static event Action<InputAction.CallbackContext> OnPauseInput;

        public static event Action<InputAction.CallbackContext> OnCancelInput;

        [Header("Input Activation States")]
        [SerializeField] private GameState gameplayInputActiveStates;
        private bool _gameplayInputsDisabled;
        [SerializeField] private GameState pauseInputActiveStates;
        private bool _pauseInputsDisabled;
        [SerializeField] private GameState uIInputActiveStates;
        private bool _uIInputsDisabled;
        private Vector2 _lastInputDirection;
        private PlayerActions _playerActions;
        private GameManager _gameManager;
        private TickManager _tickManager;
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            _playerActions = new PlayerActions();
            _playerActions.Enable();
        }
        private void OnDestroy()
        {
            if (_playerActions is not null) _playerActions.Disable();
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _tickManager = TickManager.Instance;
        }

        private void Update()
        {
            //? if the flag is not inside the active states, disable the input. (NOTE: OnAnyInput still is invoked.)
            _gameplayInputsDisabled = (gameplayInputActiveStates & _gameManager.State) != _gameManager.State;
            _pauseInputsDisabled = (pauseInputActiveStates & _gameManager.State) != _gameManager.State;
            _uIInputsDisabled = (uIInputActiveStates & _gameManager.State) != _gameManager.State;

        }

        private void OnEnable()
        {

            _playerActions.Gameplay.LayEgg.performed += LayEggAction;
            _playerActions.Gameplay.LayEgg.canceled += LayEggAction;

            _playerActions.Gameplay.Up.performed += UpAction;
            _playerActions.Gameplay.Up.canceled += UpAction;
            _playerActions.Gameplay.Down.performed += DownAction;
            _playerActions.Gameplay.Down.canceled += DownAction;
            _playerActions.Gameplay.Left.performed += LeftAction;
            _playerActions.Gameplay.Left.canceled += LeftAction;
            _playerActions.Gameplay.Right.performed += RightAction;
            _playerActions.Gameplay.Right.canceled += RightAction;

            _playerActions.Pause.Pause.performed += PauseAction;
            _playerActions.Pause.Pause.canceled += PauseAction;

            _playerActions.UI.Cancel.performed += CancelAction;
            _playerActions.UI.Cancel.canceled += CancelAction;
        }

        private void OnDisable()
        {

            _playerActions.Gameplay.LayEgg.performed -= LayEggAction;
            _playerActions.Gameplay.LayEgg.canceled -= LayEggAction;

            _playerActions.Gameplay.Up.performed -= UpAction;
            _playerActions.Gameplay.Up.canceled -= UpAction;
            _playerActions.Gameplay.Down.performed -= DownAction;
            _playerActions.Gameplay.Down.canceled -= DownAction;
            _playerActions.Gameplay.Left.performed -= LeftAction;
            _playerActions.Gameplay.Left.canceled -= LeftAction;
            _playerActions.Gameplay.Right.performed -= RightAction;
            _playerActions.Gameplay.Right.canceled -= RightAction;

            _playerActions.Pause.Pause.performed -= PauseAction;
            _playerActions.Pause.Pause.canceled -= PauseAction;

            _playerActions.UI.Cancel.performed -= CancelAction;
            _playerActions.UI.Cancel.canceled -= CancelAction;

        }


        #region  GAMEPLAY ACTIONS
        private void LayEggAction(InputAction.CallbackContext context)
        {
            OnAnyInput?.Invoke(context);
            if (_gameplayInputsDisabled) return;

            OnLayEgg?.Invoke(context);
        }

        private void UpAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _lastInputDirection = Vector2.up;
            }
            OnAnyInput?.Invoke(context);
            if (_gameplayInputsDisabled) return;
            OnMovePad?.Invoke(context, _lastInputDirection);
        }

        private void DownAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _lastInputDirection = Vector2.down;
            }
            OnAnyInput?.Invoke(context);
            if (_gameplayInputsDisabled) return;
            OnMovePad?.Invoke(context, _lastInputDirection);
        }

        private void LeftAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _lastInputDirection = Vector2.left;
            }
            OnAnyInput?.Invoke(context);
            if (_gameplayInputsDisabled) return;
            OnMovePad?.Invoke(context, _lastInputDirection);
        }

        private void RightAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _lastInputDirection = Vector2.right;
            }
            OnAnyInput?.Invoke(context);
            if (_gameplayInputsDisabled) return;
            OnMovePad?.Invoke(context, _lastInputDirection);
        }
        #endregion

        #region PAUSE 
        private void PauseAction(InputAction.CallbackContext context)
        {
            OnAnyInput?.Invoke(context);
            if (_pauseInputsDisabled) return;
            OnPauseInput?.Invoke(context);
        }
        #endregion

        #region  UI
        private void CancelAction(InputAction.CallbackContext context)
        {
            OnAnyInput?.Invoke(context);
            if (_uIInputsDisabled) return;
            OnCancelInput?.Invoke(context);
        }

        #endregion
    }
}