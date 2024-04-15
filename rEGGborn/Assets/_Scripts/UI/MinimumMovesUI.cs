using Core;
using TMPro;
using UnityEngine;

namespace UI{
    public class MinimumMovesUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI minimumMovesText;

        private void OnEnable() {
            if(!GameManager.Instance) return;
                minimumMovesText.text = GameManager.Instance.GetMinimumMoves().ToString();
        }
    }
}