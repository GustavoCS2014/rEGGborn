using System;
using Attributes;
using Core;
using Inputs;
using Interfaces;
using Objects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player{
    public class PlayerController : MonoBehaviour {
        
        public enum PlayerState{
            Alive,
            Ghost,
            Dead,
        }

        public static event Action OnPlayerDead;
        public static event Action<int> OnGhostMove;
        public static event Action OnPlayerHatched;
        public static event Action OnEggLayed;

        public static PlayerController Instance {get; private set;}
        public bool EggLayed {get; private set;}
        public PlayerState state;
        [Header("Visuals"), Space(10)]
        [SerializeField] private Transform AliveSprite;
        [SerializeField] private Transform DeadSprite;
        [Header("LayerMasks"), Space(10)]
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private LayerMask wallMask;
        [SerializeField] private LayerMask interactableMask;
        [Header("Egg"), Space(10)]
        [SerializeField] private Egg egg;
        [SerializeField, ReadOnly] private Egg layedEgg;
        [SerializeField, ReadOnly] private int ghostMoves;
        private Vector2 _direction;

        private GameManager _gameManager;
        [SerializeField] private CollisionManager _collisionManager;
        private CollisionType _collisionType;

        private void Awake() {
            if(Instance){
                Destroy(gameObject);
            }else{
                Instance = this;
            }
        }

        private void Start() {
            _gameManager = GameManager.Instance;
            InputManager.OnLayEgg += OnLayEggEvent;
            InputManager.OnMovePad += OnMovePadEvent;
            GameManager.OnMovesIncreased += OnMovesIncreasedEvent;
        }

        private void OnDestroy() {
            InputManager.OnLayEgg -= OnLayEggEvent;
            InputManager.OnMovePad -= OnMovePadEvent;
            GameManager.OnMovesIncreased -= OnMovesIncreasedEvent;
        }

        private void Update(){
            
            switch(state){
                case PlayerState.Alive:
                    ghostMoves = -1;
                    AliveSprite.gameObject.SetActive(true);
                    DeadSprite.gameObject.SetActive(false);
                break;
                case PlayerState.Ghost:
                    DeadSprite.gameObject.SetActive(true);
                    AliveSprite.gameObject.SetActive(false);
                break;
                case PlayerState.Dead:
                    
                break;
            }

        }

        private void OnMovesIncreasedEvent(int movesCount)
        {
        }

        private void OnMovePadEvent(InputAction.CallbackContext context, Vector2 input)
        {
            if(context.canceled) return;

            _direction = input;

            //? check Collisions to the block the player will move.
            _collisionType = _collisionManager.CheckCollisionAt(
                (Vector2)transform.position + _direction,
                state == PlayerState.Ghost,
                out IInteractable interactable
                );

            if(interactable is IMovable){
                var movable = interactable as IMovable;
                movable.PushTo(_direction);
                _gameManager.IncreaseMoves();
            }
            if(interactable is IEgg){
                var GhostInteractable = interactable as IEgg;
                layedEgg = null;
                EggLayed = false;
                GhostInteractable.Hatch(this);
            }

            HandleMove(_direction, out bool moved);
            if(!moved) return;
            
            //? check collisions in the block the player is standing in after moving.
            _collisionManager.CheckCollisionAt(
                transform.position,
                state == PlayerState.Ghost,
                out IInteractable bodyInteractable
                );
            
            if(bodyInteractable is IDamager){
                var damager = interactable as IDamager;
                damager.Damage(this);
            }
            
            if(bodyInteractable is IGoal){
                var goal = interactable as IGoal;
                goal.WinStage();
            }

            HandleGhostMoves();
        }

        private void OnLayEggEvent(InputAction.CallbackContext context)
        {
            if(context.canceled) return;
            if(state == PlayerState.Ghost) return;
            if(state == PlayerState.Dead) return;
            
            if(EggLayed){
                Destroy(layedEgg.gameObject);
                layedEgg = null;
                EggLayed = false;
            }
    
            EggLayed = true;
            layedEgg = Instantiate(egg, transform.position, quaternion.identity);
            OnEggLayed?.Invoke();
            _gameManager.IncreaseMoves();
        }

        private void HandleMove(Vector3 direction, out bool moved){
            moved = false;
            if(_collisionType == CollisionType.Walkable){
                transform.position += direction;
                _gameManager.IncreaseMoves();
                moved = true;
            }
        }

        private void HandleGhostMoves(){
            if(state != PlayerState.Ghost) return;
            OnGhostMove?.Invoke(ghostMoves);
            ghostMoves--;
            if(ghostMoves < 0) Die();

        }

        public void Die(){
            if(state == PlayerState.Alive){
                state = PlayerState.Ghost;
                ghostMoves = _gameManager.GetMaxGhostMoves();
                return;
            }
            if(state == PlayerState.Ghost){
                state = PlayerState.Dead;
                OnPlayerDead?.Invoke();
            }
        }

        public void Revive(){
            if(state == PlayerState.Ghost){
                state = PlayerState.Alive;
                OnPlayerHatched?.Invoke();
                return;
            }
        }


        #region DEBUG   
        private void OnGUI() {

            // GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 30;

            // if(GUILayout.Button("~ SUICIDE ~",  GUILayout.Height(100f), GUILayout.Width(300f))){
            //     Die();
            // }
        }
        private void OnDrawGizmos() {
            Vector3Int pos = Vector3Int.RoundToInt(transform.position);
            transform.position = pos;
        }
        #endregion

    }
}
