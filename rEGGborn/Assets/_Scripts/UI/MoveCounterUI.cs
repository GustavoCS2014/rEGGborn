using TMPro;
using UnityEngine;

public class MoveCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterUI;

    private void OnEnable() {
        counterUI.text = ""+GameManager.Instance.MoveCount;
        GameManager.OnMovesIncreased += OnMovesIncreasedEvent;
    }

    private void OnDisable() {
        GameManager.OnMovesIncreased -= OnMovesIncreasedEvent;
    }

    private void OnMovesIncreasedEvent(int moves)
    {
        counterUI.text = ""+moves;
    }
}
