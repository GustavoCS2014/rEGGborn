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

        public PlayerController Instance {get; private set;}
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

        private void Awake() {
            if(Instance){
                Destroy(gameObject);
            }else{
                Instance = this;
            }
        }

        private void Start() {
            // InputManager.OnMove += OnMoveEvent;
            InputManager.OnLayEgg += OnLayEggEvent;
            InputManager.OnMovePad += OnMovePadEvent;
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

        private void OnMovePadEvent(InputAction.CallbackContext context, Vector2 input)
        {
            if(context.canceled) return;

            _direction = input;
            HandleMove(_direction);
        }

        private void OnLayEggEvent(InputAction.CallbackContext context)
        {
            if(context.canceled) return;
            if(EggLayed){
                Destroy(layedEgg.gameObject);
                layedEgg = null;
                EggLayed = false;
            }

            EggLayed = true;
            layedEgg = Instantiate(egg, transform.position, quaternion.identity);
        }

        private void HandleMove(Vector2 direction){
            int collision = CheckCollisionAt((Vector2)transform.position + direction, out Collider2D collider);
            if(collision > 0){
                if(!collider.TryGetComponent(out IMovable movable)) return;
                movable.PushTo(_direction);
            }

            if(collision == 0){
                transform.position += (Vector3)direction;
                return;
            }
        }


        #region COLLISION DETECTION

        /// <summary>
        /// Returns an int that identifies what it collided with.
        /// -1 = Wall (Cannot Walk)
        /// 0 = Floor (Can walk perfectly)
        /// 1 = Movable Object (Can Push it)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private int CheckCollisionAt(Vector2 position, out Collider2D collider){
            collider = null;
            if(state == PlayerState.Dead){
                // if the player is dead and collides with an egg, revive.
                if(!(collider = Physics2D.OverlapCircle(position, .4f, interactableMask))) return 0;
                if(!collider.TryGetComponent(out IGhostInteractable interactable)) return 0;
                var newEgg = interactable as Egg;
                newEgg.Reborn(out Vector3 eggPos);
                transform.position = eggPos;
                state = PlayerState.Alive;
                return -1;
            }
            if(collider = Physics2D.OverlapCircle(position, .4f, interactableMask)) return 1;
            if(Physics2D.OverlapCircle(position, .4f, wallMask)) return -1;
            if(Physics2D.OverlapCircle(position, .4f, groundMask)) return 0;
            return -1;
        }
        #endregion

        private void Die(){
            state = PlayerState.Dead;
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

            Gizmos.color = Color.red;
            if(CheckCollisionAt((Vector2)transform.position + _direction, out Collider2D c) == 0)
                Gizmos.color = Color.green;
            
            Gizmos.DrawSphere(
                (Vector3)_direction + transform.position,
                .25f
            );
        }
        #endregion

    }
}
