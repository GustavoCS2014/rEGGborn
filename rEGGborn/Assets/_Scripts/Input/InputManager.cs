using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs{    
    public class InputManager : MonoBehaviour {
        public static InputManager Instance {get; private set;}
        public static event Action<InputAction.CallbackContext> OnAnyInput;
        public static event Action<InputAction.CallbackContext> OnLayEgg;
        public static event Action<InputAction.CallbackContext, Vector2> OnMovePad;
        private Vector2 _lastInputDirection;
        private PlayerActions _playerActions;
        private void Awake() {
            if(Instance){
                Destroy(gameObject);
            }else{
                Instance = this;
            }
            _playerActions = new PlayerActions();
            _playerActions.Enable();
        }

        private void OnEnable() {

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
            
        }

        private void OnDisable() {

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
        }
        private void LayEggAction(InputAction.CallbackContext context){
            
            OnAnyInput?.Invoke(context);
            OnLayEgg?.Invoke(context);
        }

        private void UpAction(InputAction.CallbackContext context){
            if(context.performed){
                _lastInputDirection = Vector2.up;
            }
            OnAnyInput?.Invoke(context);
            OnMovePad?.Invoke(context, _lastInputDirection);
        }

        private void DownAction(InputAction.CallbackContext context){
            if(context.performed){
                _lastInputDirection = Vector2.down;
            }
            OnAnyInput?.Invoke(context);
            OnMovePad?.Invoke(context, _lastInputDirection);
        }

        private void LeftAction(InputAction.CallbackContext context){
            if(context.performed){
                _lastInputDirection = Vector2.left;
            }
            OnAnyInput?.Invoke(context);
            OnMovePad?.Invoke(context, _lastInputDirection);
        }

        private void RightAction(InputAction.CallbackContext context){
            if(context.performed){
                _lastInputDirection = Vector2.right;
            }
            OnAnyInput?.Invoke(context);
            OnMovePad?.Invoke(context, _lastInputDirection);
        }
    }
}