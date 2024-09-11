using System;
using Reggborn.Core;
using Reggborn.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Reggborn.UI
{
    public class PauseUI : SceneLevelUI, IUserInterface
    {
        public static event Action OnGamePaused;
        public static event Action OnGameResumed;
        [SerializeField] private Transform pausePanel;

        [SerializeField] private Button resumeBTN;

        private GameState _lastState;

        private void OnEnable()
        {
            InputManager.OnPauseInput += OnPauseInputEvent;
        }

        private void OnDisable()
        {
            InputManager.OnPauseInput -= OnPauseInputEvent;
        }

        private void OnPauseInputEvent(InputAction.CallbackContext context)
        {
            if (context.canceled) return;
            if (GameManager.Instance.GetCurrentState() == GameState.Paused)
            {
                Close();
                return;
            }
            Show();
        }

        public override void Close()
        {
            base.Close();

            pausePanel.gameObject.SetActive(false);
            GameManager.Instance.ChangeState(_lastState);
            OnGameResumed?.Invoke();
        }

        public override void Show()
        {
            base.Show();

            resumeBTN.onClick.AddListener(ResumeGame);
            pausePanel.gameObject.SetActive(true);
            defaultButton.Select();

            _lastState = GameManager.Instance.GetCurrentState();
            GameManager.Instance.ChangeState(GameState.Paused);
            OnGamePaused?.Invoke();
        }

        private void ResumeGame()
        {
            Close();
        }
    }
}