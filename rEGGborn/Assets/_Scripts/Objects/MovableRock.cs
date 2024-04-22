using System;
using Core;
using Player;
using UnityEngine;
namespace Objects.Movable{
    public sealed class MovableRock : GridObject{
        public override uint CollisionPriority { get; protected set; } = 1;
        public override CollisionType Type { get; protected set; } = CollisionType.Wall;
        public override bool GhostInteractable { get; protected set; } = false;

        [SerializeField] private NewCollisionManager collisionManager;

        protected override void Start(){
            base.Start();
        }

        public override void Interact(PlayerController player){
            Vector3 direction = transform.position - player.transform.position; 
            if(VerifyMove(transform.position + direction)){
                transform.position += Vector3Int.RoundToInt(direction);
                base.InteractionSuccessful();
            }
            
        }

        public bool VerifyMove(Vector2 pushDirection){
            CollisionType type = collisionManager.DetectCollisionAt(pushDirection, out GridObject gridObject);
            
            if(type is CollisionType.Wall) return false;
            if(gridObject is Spikes) return true;
            if(gridObject is null) return true;
            return false;
        }

        public Transform GetTransform() => transform;
        public GameObject GetGameObject() => gameObject;
    }
}