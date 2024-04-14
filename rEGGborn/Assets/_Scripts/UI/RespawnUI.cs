using System;
using Core;
using Player;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class RespawnUI : MonoBehaviour, IUserInterface {
    
    [SerializeField] private Transform respawnPanel;
    [SerializeField] private Button defaultButton;

    private void Start() {
        PlayerController.OnPlayerDeadEvent += OnPlayerDeadEvent;
    }

    private void OnDestroy() {
        PlayerController.OnPlayerDeadEvent -= OnPlayerDeadEvent;
    }

    private void OnPlayerDeadEvent()
    {
        GameManager.Instance.ChangeState(GameState.NextOrRetryScene);
        respawnPanel.gameObject.SetActive(true);
        defaultButton.Select();
    }

    public void Show() {
        respawnPanel.gameObject.SetActive(true);
        defaultButton.Select();
    }

    public void Close()
    {
        respawnPanel.gameObject.SetActive(false);
    }
}