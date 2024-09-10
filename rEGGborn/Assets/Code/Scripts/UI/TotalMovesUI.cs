using Reggborn.Core;
using TMPro;
using UnityEngine;

namespace Reggborn.UI
{
    public class TotalMovesUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI counter;

        private void Start()
        {
            counter.text = GameManager.Instance.GetTotalMoveCount().ToString();
        }
    }
}