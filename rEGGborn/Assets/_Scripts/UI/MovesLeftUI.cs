using System;
using Player;
using TMPro;
using UnityEngine;
    
namespace UI{
    public class MovesLeftUI : MonoBehaviour, IUserInterface
    {
        [SerializeField] private Transform movesLeftContainer;
        [SerializeField] private TextMeshProUGUI movesLeftText;
        [SerializeField] private Color warningColor;
        private Color _defaultColor;

        private void Start() {
            _defaultColor = movesLeftText.color;
            PlayerController.OnGhostMove += OnGhostMoveEvent;
            PlayerController.OnPlayerHatched += OnPlayerHatchedEvent;
        }

        private void OnDisable() {
            PlayerController.OnGhostMove -= OnGhostMoveEvent;
            PlayerController.OnPlayerHatched -= OnPlayerHatchedEvent;
        }

        private void OnPlayerHatchedEvent()
        {
            Close();
        }

        private void OnGhostMoveEvent(int movesLeft)
        {
            Show();
            // if(movesLeft < 0){
            //     Close();
            // }

            movesLeftText.color = _defaultColor;
            int lastThree = 3;
            if(movesLeft <= lastThree){
                movesLeftText.color = warningColor;
            }
            movesLeftText.text = movesLeft.ToString();
        }

        public void Close() {
            movesLeftContainer.gameObject.SetActive(false);
        }

        public void Show() {
            movesLeftContainer.gameObject.SetActive(true);
        }
    }
}