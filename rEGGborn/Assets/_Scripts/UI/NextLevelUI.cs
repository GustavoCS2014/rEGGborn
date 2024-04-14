using Core;
using UnityEngine;

public class NextLevelUI : MonoBehaviour {
    
    [SerializeField] private Transform nextLevelPanel;

    private void Start() {
        Goal.OnGoalEnter += OnGoalEnterEvent;
    }
    private void OnDestroy() {
        Goal.OnGoalEnter -= OnGoalEnterEvent;
    }
    private void OnGoalEnterEvent()
    {
        GameManager.Instance.ChangeState(GameState.NextOrRetryScene);
        nextLevelPanel.gameObject.SetActive(true);
    }
}