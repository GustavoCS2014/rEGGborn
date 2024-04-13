using System;
using Inputs;
using Interfaces;
using Unity.Mathematics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player{
    public class PlayerController : MonoBehaviour {
        
        public enum PlayerState{
            Alive,
            Dead,
        }

        public static PlayerController Instance {get; private set;}
        public bool EggLayed {get; private set;}
        public PlayerState state;
        [Header("Visuals"), Space(10)]
        [SerializeField] private SpriteRenderer AliveSprite;
        [SerializeField] private SpriteRenderer DeadSprite;
        [Header("LayerMasks"), Space(10)]
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private LayerMask wallMask;
        [SerializeField] private LayerMask interactableMask;
        [Header("Egg"), Space(10)]
        [SerializeField] private Egg egg;
        [SerializeField] private Egg layedEgg;
        private Vector2 _direction;

        private GameManager _gameManager;
        private CollisionManager _collisionHandler;
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
            _collisionHandler = CollisionManager.Instance;
            // InputManager.OnMove += OnMoveEvent;
            InputManager.OnLayEgg += OnLayEggEvent;
            InputManager.OnMovePad += OnMovePadEvent;
            GameManager.OnMovesIncreased += OnMovesIncreasedEvent;
        }

        private void OnDestroy() {
            // InputManager.OnMove -= OnMoveEvent;
            InputManager.OnLayEgg -= OnLayEggEvent;
            InputManager.OnMovePad -= OnMovePadEvent;
        }

        private void Update(){
            
            switch(state){
                case PlayerState.Alive:
                    AliveSprite.enabled = true;
                    DeadSprite.enabled = false;
                break;
                case PlayerState.Dead:
                    DeadSprite.enabled = true;
                    AliveSprite.enabled = false;
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
            _collisionType = _collisionHandler.CheckCollisionAt(
                (Vector2)transform.position + _direction,
                state == PlayerState.Dead,
                out IInteractable interactable
                );

            if(interactable is IMovable){
                var movable = interactable as IMovable;
                movable.PushTo(_direction);
                _gameManager.IncreaseMoves();
            }
            if(interactable is IDamager){
                var damager = interactable as IDamager;
                damager.Damage(this);
            }
            if(interactable is IEgg){
                var GhostInteractable = interactable as IEgg;
                layedEgg = null;
                EggLayed = false;
                GhostInteractable.Hatch(this);
            }

            HandleMove(_direction);
        }

        private void OnLayEggEvent(InputAction.CallbackContext context)
        {
            if(context.canceled) return;
            if(state == PlayerState.Dead) return;
            
            if(EggLayed){
                Destroy(layedEgg.gameObject);
                layedEgg = null;
                EggLayed = false;
            }
    
            EggLayed = true;
            layedEgg = Instantiate(egg, transform.position, quaternion.identity);
            _gameManager.IncreaseMoves();
        }

        private void HandleMove(Vector3 direction){
            if(_collisionType == CollisionType.Walkable){
                transform.position += direction;
                _gameManager.IncreaseMoves();
            }
        }

        public void Die(){
            if(state == PlayerState.Alive){
                state = PlayerState.Dead;
                return;
            }
        }

        public void Revive(){
            if(state == PlayerState.Dead){
                state = PlayerState.Alive;
                return;
            }
        }


        #region DEBUG   
        private void OnGUI() {

            GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 30;
            if(GUILayout.Button("~ SUICIDE ~",  GUILayout.Height(100f), GUILayout.Width(300f))){
                Die();
                // layedEgg.Reborn(out Vector3 position);
                // EggLayed = false;
                // transform.position = position;
            }
        }
        private void OnDrawGizmos() {
        }
        #endregion

    }
}
