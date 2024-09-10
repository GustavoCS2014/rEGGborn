using Reggborn.Inputs;
using Reggborn.Music;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Reggborn.UI
{
    public class ConfigUI : MonoBehaviour, IUserInterface
    {
        [SerializeField] private Transform container;
        [SerializeField] private CanvasGroup mainMenuButtonGroup;
        [SerializeField] private Button selectedWhenOpened;
        [SerializeField] private Button selectedWhenClosed;
        [SerializeField] private TextMeshProUGUI musicVolumeText;


        private void Start()
        {
            InputManager.OnCancelInput += OnCancelInputEvent;
            MusicManager.OnVolumeChanged += UpdateVolumeText;
            UpdateVolumeText(MusicManager.Instance.GetMusicVolume());
        }

        private void OnDisable()
        {
            InputManager.OnCancelInput -= OnCancelInputEvent;
            MusicManager.OnVolumeChanged -= UpdateVolumeText;
        }

        public void UpdateVolumeText(float value)
        {
            musicVolumeText.text = string.Format($"{Mathf.RoundToInt(value * 100)}%");
        }

        private void OnCancelInputEvent(InputAction.CallbackContext context)
        {
            if (context.canceled) return;
            Close();
        }

        public void Close()
        {
            container.gameObject.SetActive(false);
            if (mainMenuButtonGroup is null) return;
            mainMenuButtonGroup.interactable = true;
            selectedWhenClosed.Select();
        }

        public void Show()
        {
            container.gameObject.SetActive(true);
            if (selectedWhenOpened is null) return;
            selectedWhenOpened.Select();
        }
    }
}