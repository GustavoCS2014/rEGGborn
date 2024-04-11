using System;
using Inputs;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player{
    public class PlayerController : MonoBehaviour {
        
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private LayerMask wallMask;
        [SerializeField] private LayerMask interactableMask;

        private Vector2 _direction;

        private void Start() {
            InputManager.OnMove += OnMoveEvent;
            InputManager.OnLayEgg += OnLayEggEvent;
        }

        private void OnDestroy() {
            InputManager.OnMove -= OnMoveEvent;
            InputManager.OnLayEgg -= OnLayEggEvent;
        }

        private void OnLayEggEvent(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        private void OnMoveEvent(InputAction.CallbackContext context)
        {
            if(context.canceled) return;
            Vector2 normalizedDirection = context.ReadValue<Vector2>().normalized;
            _direction = Vector2.zero;
            if(Mathf.Abs(normalizedDirection.x) > .9f){
                _direction += new Vector2(context.ReadValue<Vector2>().x, 0);
            }
            if(Mathf.Abs(normalizedDirection.y) > .9f){
                _direction += new Vector2(0, context.ReadValue<Vector2>().y);
            }
            
            // if(CheckCollisions(_direction, out IMovable? movable)){
            //     if(!movable.CanBePushed(_direction)) return;
            //     movable.Push(_direction);
            //     return;
            // }
            HandleMove(_direction);
        }

        private void Update(){
            // Vector2 direction = GetMoveDirection();
            Debug.DrawRay(transform.position, _direction, Color.red);
            // if(CheckCollisions(direction, out IMovable movable)){
            //     if(!movable.CanBePushed(direction)) return;
            //     movable.Push(direction);
            //     return;
            // }
            // HandleMove(direction);
        }

        // private Vector3 GetMoveDirection(){
        //     if(Input.GetKeyDown(KeyCode.W)){
        //         return Vector2.up;
        //     }
        //     if(Input.GetKeyDown(KeyCode.S)){
        //         return Vector2.down;
        //     }
        //     if(Input.GetKeyDown(KeyCode.D)){
        //         return Vector2.right;
        //     }
        //     if(Input.GetKeyDown(KeyCode.A)){
        //         return Vector2.left;
        //     }
        //     return Vector2.zero;
        // }

        // private bool CheckCollisions(Vector2 direction, out IMovable? movableObject){
        //     movableObject = null;
        //     Collider2D col = Physics2D.OverlapCircle((Vector2)transform.position + direction, .5f, interactableMask);
        //     if(col.TryGetComponent<IMovable>(out movableObject)){
        //         Debug.Log($"true");
        //         return true;
        //     }
        //     return false;
        // }

        private void HandleMove(Vector2 direction){
            if(Physics2D.OverlapBox((Vector2)transform.position + direction, Vector2.one * .5f, 0f, wallMask)){
                return;
            }
            if(Physics2D.OverlapBox((Vector2)transform.position + direction, Vector2.one * .5f, 0f, groundMask)){
                transform.position += (Vector3)direction;
                return;
            }
        }

    }
}
