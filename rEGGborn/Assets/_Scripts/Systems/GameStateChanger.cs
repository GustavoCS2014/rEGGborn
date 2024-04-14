using Core;
using UnityEngine;

public class GameStateChanger : MonoBehaviour {
    [Tooltip("WARNING: Choose only one.")]
    [SerializeField] private GameState targetState;

    public void ChangeCurrentState(){
        GameManager.Instance.ChangeState(targetState);
    }
}