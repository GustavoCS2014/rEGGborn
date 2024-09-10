using Reggborn.Player;
using UnityEngine;

namespace Reggborn.UI
{
    public class RestartHintUI : MonoBehaviour, IUserInterface
    {
        [SerializeField] private Transform container;

        private void Start()
        {
            PlayerController.OnShowRespawnHint += Show;
        }

        private void OnDisable()
        {
            PlayerController.OnShowRespawnHint -= Show;
        }

        public void Close()
        {
            container.gameObject.SetActive(false);
        }

        public void Show()
        {
            container.gameObject.SetActive(true);
        }
    }
}