using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterUI;

    private void Start() {
        GameManager.OnMovesIncreased += OnMovesIncreasedEvent;
    }

    private void OnMovesIncreasedEvent(int moves)
    {
        counterUI.text = ""+moves;
    }
}
