using System;
using Core;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class RespawnUI : MonoBehaviour {
    
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
        Debug.Log($"ded");  
        GameManager.Instance.ChangeState(GameState.NextOrRetryScene);
        respawnPanel.gameObject.SetActive(true);
        defaultButton.Select();
    }
}