using Reggborn.Core;
using TMPro;
using Reggborn.UI;
using UnityEngine;

public class MoveCounterUI : MonoBehaviour, IUserInterface
{
    [SerializeField] private TextMeshProUGUI counterUI;
    [SerializeField] private Transform counterContainer;

    private void Start()
    {
        GameManager.OnMovesIncreased += OnMovesIncreasedEvent;
        PauseUI.OnGamePaused += OnGamePausedEvent;
        PauseUI.OnGameResumed += OnGameResumedEvent;
        counterUI.text = GameManager.Instance.MoveCount.ToString();
    }


    private void OnGameResumedEvent()
    {
        Show();
    }

    private void OnGamePausedEvent()
    {
        Close();
    }

    private void OnDisable()
    {
        GameManager.OnMovesIncreased -= OnMovesIncreasedEvent;
        PauseUI.OnGamePaused -= OnGamePausedEvent;
        PauseUI.OnGameResumed -= OnGameResumedEvent;
    }

    private void OnMovesIncreasedEvent(int moves)
    {
        counterUI.text = moves.ToString();
    }

    public void Show()
    {
        if (counterContainer is null) return;
        counterContainer.gameObject.SetActive(true);
    }

    public void Close()
    {
        if (counterContainer is null) return;
        counterContainer.gameObject.SetActive(false);
    }
}
