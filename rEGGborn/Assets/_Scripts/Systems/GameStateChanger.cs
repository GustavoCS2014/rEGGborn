using UnityEngine;

public class GameStateChanger : MonoBehaviour {
    [Tooltip("WARNING: Choose only one.")]
    [SerializeField] private GameStates targetState;

    public void ChangeCurrentState(){
        GameManager.Instance.ChangeState(targetState);
    }
}