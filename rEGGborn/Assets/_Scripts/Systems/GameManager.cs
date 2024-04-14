using System;
using System.Collections.Generic;
using Attributes;
using UnityEngine;

namespace Core{
    public class GameManager : MonoBehaviour {
        public static event Action<int> OnMovesIncreased;

        public static GameManager Instance {get; private set;}
        public int MoveCount {get; private set;}
        public Dictionary<uint, int> MoveCountHistory = new Dictionary<uint, int>(); 
        public GameState State {get; private set;}
        [Tooltip("Only use when the current scene isn't a level.")]
        [SerializeField] private SceneSettings startingScene;
        [SerializeField, ReadOnly] private GameState currentState;
        [SerializeField, ReadOnly] private SceneSettings currentScene;

        private void Awake() {
            if(Instance){
                Destroy(gameObject);
            }else{
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
        }

        private void Start() {
            currentScene = startingScene;
            State = currentScene.StartingState;
            currentState = State;
        }

        private void OnDestroy() {
        }

        private void Update(){
            switch(State){
                case GameState.MainMenu:
                    
                break;
                case GameState.Playing:
                
                break;
                case GameState.Paused:
                    
                break;
                case GameState.NextOrRetryScene:
                    
                break;
                case GameState.GameOver:
                
                break;
            }
        }

        /// <summary>
        /// Resets the move count and stores the last movecount in the history.
        /// </summary>
        public void ResetMoveCount(){
            if(currentScene.LevelIndex is not null)
                MoveCountHistory.TryAdd(currentScene.LevelIndex.Value, MoveCount);
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
        public int GetMoveCountAtGame(uint game) {
            MoveCountHistory.TryGetValue(game, out int moves);
            return moves;
        }

        /// <summary>
        /// Get the sum of all moves across games.
        /// </summary>
        /// <returns></returns>
        public int GetTotalMoveCount() {
            int moves = 0;
            for(uint i = 0; i < MoveCountHistory.Count; i++){
                MoveCountHistory.TryGetValue(i, out int moveCount);
                moves += moveCount;
            }
            return moves;
        }

        public void IncreaseMoves(){
            MoveCount ++;
            OnMovesIncreased?.Invoke(MoveCount);
        }

        public void SetScene(SceneSettings scene){
            currentScene = scene;
            ChangeState(currentScene.StartingState);
        }

        public SceneSettings GetCurrentScene() => currentScene; 

        public void ChangeState(GameState state){
            State = state;
            currentState = State;
        }

        public GameState GetCurrentState() => currentState;

        public void ExitGame(){
            Application.Quit();
            #if UNITY_EDITOR
            Debug.LogWarning($"Quiting Game");
            #endif
        }
    }
}