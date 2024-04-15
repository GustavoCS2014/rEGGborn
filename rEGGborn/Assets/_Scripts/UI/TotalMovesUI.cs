using Core;
using TMPro;
using UnityEngine;
    
namespace UI{
    public class TotalMovesUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI counter;

        private void Start() {
            counter.text = GameManager.Instance.GetTotalMoveCount().ToString();
        }        
    }
}