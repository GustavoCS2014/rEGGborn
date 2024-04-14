using System;
using Interfaces;
using UnityEngine;
public class Goal : MonoBehaviour, IGoal
{
    public static event Action OnGoalEnter;

    public GameObject GetGameObject() => gameObject;

    public Transform GetTransform() => transform;

    public void WinStage()
    {
        OnGoalEnter?.Invoke();
    }
}