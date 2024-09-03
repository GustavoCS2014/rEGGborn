#if UNITY_EDITOR
using System;
using Core;
using Inputs;
using Player;
using UnityEngine;

public class EventTester : MonoBehaviour
{
    private int _executionOrder = 0;
    private void Start()
    {
        GameManager.OnMovesIncreased += MovesIncresed;
        TickManager.OnTick += Tick;
        PlayerController.OnSuccessfulAction += SuccessfulAction;
        InputManager.OnAnyInput += OnAnyInputEvent;
    }

    private void OnDisable()
    {
        GameManager.OnMovesIncreased -= MovesIncresed;
        TickManager.OnTick -= Tick;
        PlayerController.OnSuccessfulAction -= SuccessfulAction;
        InputManager.OnAnyInput -= OnAnyInputEvent;
    }

    private void OnAnyInputEvent(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _executionOrder++;
        Debug.Log($"{_executionOrder}.- OnAnyInput-{context.phase}");
    }

    private void SuccessfulAction()
    {
        _executionOrder++;
        Debug.Log($"{_executionOrder}.- OnSuccessfulActionEvent");
    }

    private void Tick(int obj)
    {
        _executionOrder++;
        Debug.Log($"{_executionOrder}.- OnTickEvent");
    }

    private void MovesIncresed(int obj)
    {
        _executionOrder++;
        Debug.Log($"{_executionOrder}.- OnMovesIncreasedEvent");
    }
}
#endif