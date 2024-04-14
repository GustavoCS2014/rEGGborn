using Core;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelUI : MonoBehaviour {
    
    [SerializeField] private Transform nextLevelPanel;
    [SerializeField] private Button defaultButton;

    private void Start() {
        Goal.OnGoalEnter += OnGoalEnterEvent;
    }
    private void OnDestroy() {
        Goal.OnGoalEnter -= OnGoalEnterEvent;
    }
    private void OnGoalEnterEvent()
    {
        GameManager.Instance.ChangeState(GameState.NextOrRetryScene);
        defaultButton.Select();
        nextLevelPanel.gameObject.SetActive(true);
    }
}