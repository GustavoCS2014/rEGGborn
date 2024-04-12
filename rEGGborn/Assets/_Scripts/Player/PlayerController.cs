using System;
using System.Collections.Generic;
using System.Linq;
using Inputs;
using Interfaces;
using Unity.Mathematics;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

namespace Player{
    public class PlayerController : MonoBehaviour {
        
        public enum PlayerState{
            Alive,
            Dead,
        }

        public bool EggLayed {get; private set;}
        public PlayerState state;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private LayerMask wallMask;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private Egg egg;
        [SerializeField] private Egg layedEgg;
        private Vector2 _direction;

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
        private int CheckCollisionAt(Vector2 position){
            float checkSize = .2f;
            if(Physics2D.OverlapCircle(position, checkSize, interactableMask)) return 1; 
            if(Physics2D.OverlapCircle(position, checkSize, wallMask)) return -1;
            if(Physics2D.OverlapCircle(position, checkSize, groundMask)) return 0;
            return -1;
        }

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
            if(collider = Physics2D.OverlapCircle(position, .4f, interactableMask)) return 1;
            if(Physics2D.OverlapCircle(position, .4f, wallMask)) return -1;
            if(Physics2D.OverlapCircle(position, .4f, groundMask)) return 0;
            return -1;
        }
        #endregion

        private void OnGUI() {

            GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 30;
            if(GUILayout.Button("~ SUICIDE ~",  GUILayout.Height(100f), GUILayout.Width(300f))){
                layedEgg.Reborn(out Vector3 position);
                EggLayed = false;
                transform.position = position;
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            if(CheckCollisionAt((Vector2)transform.position + _direction) == 0)
                Gizmos.color = Color.green;
            
            Gizmos.DrawSphere(
                (Vector3)_direction + transform.position,
                .25f
            );
        }

    }
}
