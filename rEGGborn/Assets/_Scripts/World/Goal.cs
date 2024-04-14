using System;
using Interfaces;
using UnityEngine;
public class Goal : MonoBehaviour, IGoal
{
    public static event Action OnGoalEnter;
    public void WinStage()
    {
        OnGoalEnter?.Invoke();
        GameManager.Instance.ChangeState(GameStates.NextScene);
    }
}