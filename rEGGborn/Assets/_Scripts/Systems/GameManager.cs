using System;
using System.Collections.Generic;
using Attributes;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static event Action<int> OnMovesIncreased;

    public static GameManager Instance {get; private set;}
    public int MoveCount {get; private set;}
    public int GameCount {get; private set;}
    public Dictionary<int, int> MoveCountHistory = new Dictionary<int, int>(); 
    public GameStates State {get; private set;}
    [SerializeField] private GameStates startingState;
    [SerializeField, ReadOnly] private GameStates currentState;

    private void Awake() {
        if(Instance){
            Destroy(gameObject);
        }else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        GameCount = 0;
        State = startingState;
    }

    private void Start() {
        currentState = State;
    }

    private void OnDestroy() {
    }

    private void Update(){
        switch(State){
            case GameStates.MainMenu:
                
            break;
            case GameStates.Playing:
            
            break;
            case GameStates.Paused:
                
            break;
            case GameStates.NextScene:
                
            break;
            case GameStates.GameOver:
            
            break;
        }
    }

    /// <summary>
    /// Resets the move count and stores the last movecount in the history.
    /// </summary>
    public void ResetMoveCount(){
        MoveCountHistory.TryAdd(GameCount, MoveCount);
        GameCount++;
        MoveCount = 0;
    }

    /// <summary>
    /// Resets the move count.
    /// </summary>
    public void ResetMoveCountWhitoutStoring() => MoveCount = 0;

    /// <summary>
    /// Get current move count;
    /// </summary>
    public int GetMoveCount() => MoveCount;

    /// <summary>
    /// Get move count at the game specified.
    /// </summary>
    /// <param name="game">Game index</param>
    public int GetMoveCountAtGame(int game) {
        MoveCountHistory.TryGetValue(game, out int moves);
        return moves;
    }

    /// <summary>
    /// Get the sum of all moves across games.
    /// </summary>
    /// <returns></returns>
    public int GetTotalMoveCount() {
        int moves = 0;
        for(int i = 0; i < MoveCountHistory.Count; i++){
            MoveCountHistory.TryGetValue(i, out int moveCount);
            moves += moveCount;
        }
        return moves;
    }

    public void IncreaseMoves(){
        MoveCount ++;
        OnMovesIncreased?.Invoke(MoveCount);
    }

    public void ChangeState(GameStates state){
        State = state;
        currentState = State;
    }

    public void ExitGame(){
        Application.Quit();
        #if UNITY_EDITOR
        Debug.LogWarning($"Quiting Game");
        #endif
    }
}
