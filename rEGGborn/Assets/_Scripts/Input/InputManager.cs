using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs{    
    public class InputManager : MonoBehaviour {
        public static InputManager Instance {get; private set;}

        public static event Action<InputAction.CallbackContext> OnMove;
        public static event Action<InputAction.CallbackContext> OnLayEgg;

        private PlayerActions _playerActions;
        private void Awake() {
            _playerActions = new PlayerActions();
            _playerActions.Enable();

            if(Instance){
                Destroy(gameObject);
            }else{
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        private void OnEnable() {
            _playerActions.Gameplay.Move.performed += MoveAction;
            _playerActions.Gameplay.Move.canceled += MoveAction;

            _playerActions.Gameplay.LayEgg.performed += LayEggAction;
            _playerActions.Gameplay.LayEgg.canceled += LayEggAction;
        }

        private void OnDisable() {
            _playerActions.Gameplay.Move.performed -= MoveAction;
            _playerActions.Gameplay.Move.canceled -= MoveAction;

            _playerActions.Gameplay.LayEgg.performed -= LayEggAction;
            _playerActions.Gameplay.LayEgg.canceled -= LayEggAction;
        }

        private void MoveAction(InputAction.CallbackContext context){
            OnMove?.Invoke(context);
        }

        private void LayEggAction(InputAction.CallbackContext context){
            OnLayEgg?.Invoke(context);
        }
    }
}