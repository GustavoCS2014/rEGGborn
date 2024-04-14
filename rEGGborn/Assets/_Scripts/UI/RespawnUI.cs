using System;
using Core;
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
        Debug.Log($"ded");  
        GameManager.Instance.ChangeState(GameState.NextOrRetryScene);
        RespawnPanel.gameObject.SetActive(true);
    }
}