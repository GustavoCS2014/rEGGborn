using System;
using Core;
using Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI{
    public class PauseUI : MonoBehaviour, IUserInterface
    {
        public static event Action OnGamePaused;
        public static event Action OnGameResumed;
        [SerializeField] private Transform pausePanel;
        [SerializeField] private Button defaultButton;
        private GameState _lastState;

        private void OnEnable() {
            InputManager.OnPauseInput += OnPauseInputEvent;
        }
        
        private void OnDisable() {
            InputManager.OnPauseInput -= OnPauseInputEvent;
        }

        private void OnPauseInputEvent(InputAction.CallbackContext context){
            if(context.canceled) return;
            if(GameManager.Instance.GetCurrentState() == GameState.Paused){
                Close();
                return;
            }
            Show();
        }

        public void Close(){
            pausePanel.gameObject.SetActive(false);
            GameManager.Instance.ChangeState(_lastState);
            OnGameResumed?.Invoke();
        }

        public void Show() {
            pausePanel.gameObject.SetActive(true);
            defaultButton.Select();
            _lastState = GameManager.Instance.GetCurrentState();
            GameManager.Instance.ChangeState(GameState.Paused);
            OnGamePaused?.Invoke();
        }
    }
}