using Core;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelUI : MonoBehaviour, IUserInterface {
    
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
        Show();
    }
    
    public void Show() {
        nextLevelPanel.gameObject.SetActive(true);
        defaultButton.Select();
    }

    public void Close()
    {
        nextLevelPanel.gameObject.SetActive(false);
    }
}