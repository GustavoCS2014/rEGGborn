using System;
using Attributes;
using Player;
using UI;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static event Action<int> OnMovesIncreased;
        public static event Action<GameState> OnStateChanged;

        public static GameManager Instance { get; private set; }
        public int MoveCount { get; private set; }
        public int TotalMoveCount { get; private set; }
        public GameState State { get; private set; }
        [Tooltip("Only use when the current scene isn't a level.")]
        [SerializeField] private SceneSettings startingScene;
        [SerializeField, ReadOnly] private GameState currentState;
        [SerializeField, ReadOnly] private SceneSettings currentScene;
        [SerializeField] private RestartHintUI hintUI;


        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
        }

        private void Start()
        {
            //? Setting the starting scene.
            currentScene = startingScene;
            ChangeState(currentScene.StartingState);
            OnStateChanged?.Invoke(currentState);

            PlayerController.OnSuccessfulAction += OnSuccessfulActionEvent;

            // State = currentScene.StartingState;
            // currentState = State;
        }

        private void OnDisable()
        {
            PlayerController.OnSuccessfulAction -= OnSuccessfulActionEvent;

        }

        private void OnSuccessfulActionEvent() => IncreaseMoves();


        private void Update()
        {
            switch (State)
            {
                case GameState.MainMenu:

                    break;

                case GameState.Playing:
                    break;

                case GameState.Paused:
                    // hintUI.Close();
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
        public void ResetMoveCount()
        {
            TotalMoveCount += MoveCount;
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
        /// Gets the minimum moves requiered to complete the level.
        /// </summary>
        public int GetMinimumMoves()
        {
            if (currentScene.MinimumMoves is null) return -1;
            return (int)currentScene.MinimumMoves.Value;
        }

        /// <summary>
        /// Gets the max amount of moves the player can make when ghost.
        /// </summary>
        /// <returns></returns>
        public int GetMaxGhostMoves()
        {
            if (currentScene.GhostMaxMoves is null) return -1;
            return (int)currentScene.GhostMaxMoves.Value;
        }

        /// <summary>
        /// Get the sum of all moves across games.
        /// </summary>
        /// <returns></returns>
        public int GetTotalMoveCount() => TotalMoveCount;

        public void IncreaseMoves()
        {
            MoveCount++;
            OnMovesIncreased?.Invoke(MoveCount);
        }

        public void SetScene(SceneSettings scene)
        {
            currentScene = scene;
            ChangeState(currentScene.StartingState);
        }

        public SceneSettings GetCurrentScene() => currentScene;

        public void ChangeState(GameState state)
        {
            State = state;
            currentState = State;
            // OnStateChanged?.Invoke(currentState);
        }

        public GameState GetCurrentState() => currentState;

        public void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            Debug.LogWarning($"Quiting Game");
#endif
        }
    }
}