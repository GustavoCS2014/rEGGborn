using UnityEngine;

namespace Reggborn.Core
{
    [CreateAssetMenu(fileName = "SceneSettings", menuName = "SceneSettings /New Scene Settings", order = 0)]
    public class SceneSettings : ScriptableObject
    {

        [Tooltip("The Scene of the level.")]
        [SerializeField] private SceneField scene;
        public SceneField Scene => scene;

        [Tooltip("The StartingState of the level.")]
        [SerializeField] private GameState startingState;
        public GameState StartingState => startingState;

        [Space(20)]
        [Header("Specific to level Scenes (-1 means disabled)")]

        [Tooltip("The minimum requiered moves to complete the level.")]
        [SerializeField] private int minimumMoves = -1;
        public uint? MinimumMoves => minimumMoves < 0 ? null : (uint)minimumMoves;

        [Tooltip("The Max amount of moves the player can make when dead.")]
        [SerializeField] private int ghostMaxMoves = -1;
        public uint? GhostMaxMoves => ghostMaxMoves < 0 ? null : (uint)ghostMaxMoves;
    }
}