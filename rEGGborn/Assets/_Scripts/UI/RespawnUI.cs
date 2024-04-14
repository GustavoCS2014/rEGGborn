using System;
using Player;
using UnityEngine;

public class RespawnUI : MonoBehaviour {
    
    [SerializeField] private Transform RespawnPanel;

    private void Start() {
        PlayerController.OnPlayerDeadEvent += OnPlayerDeadEvent;
    }

    private void OnDestroy() {
        PlayerController.OnPlayerDeadEvent -= OnPlayerDeadEvent;
    }

    private void OnPlayerDeadEvent()
    {
        GameManager.Instance.ChangeState(GameStates.NextOrRetryScene);
        RespawnPanel.gameObject.SetActive(true);
    }
}