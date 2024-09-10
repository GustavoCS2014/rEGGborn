using Reggborn.Core;
using TMPro;
using UnityEngine;

namespace Reggborn.UI
{
    public class MinimumMovesUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI minimumMovesText;

        private void OnEnable()
        {
            if (!GameManager.Instance) return;
            minimumMovesText.text = GameManager.Instance.GetMinimumMoves().ToString();
        }
    }
}