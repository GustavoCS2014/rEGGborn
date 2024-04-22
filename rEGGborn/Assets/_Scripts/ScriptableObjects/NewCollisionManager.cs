using UnityEngine;
using Objects;
namespace System{

    [CreateAssetMenu(fileName = "New Collision Manager", menuName = "New Collision Manager")]
    public class NewCollisionManager : ScriptableObject{

        [SerializeField] private LayerMask GridObjects;
        [SerializeField] private LayerMask Grounds;
        [SerializeField] private LayerMask Walls;
        private Collider2D[] _colliders;


        public CollisionType DetectCollisionAt(Vector2 position,out GridObject gridObject){
            gridObject = null;
            _colliders = new Collider2D[10];

            _colliders = Physics2D.OverlapCircleAll(position, .2f, GridObjects);

            //? if the overlapCircle didn't find any gridObject, check if its on ground.
            if(_colliders.Length == 0){
                if(Physics2D.OverlapCircle(position, .2f, Walls)) return CollisionType.Wall;
                if(Physics2D.OverlapCircle(position, .2f, Grounds)) return CollisionType.Walkable;
                return CollisionType.Wall;
            }

            gridObject = FindHighestPriorityObject(_colliders, false);
            
            return gridObject.Type;
        }

        public CollisionType DetectCollisionAt(Vector2 position,bool playerIsDead ,out GridObject gridObject){
            gridObject = null;
            _colliders = new Collider2D[10];

            _colliders = Physics2D.OverlapCircleAll(position, .2f, GridObjects);

            //? if the overlapCircle didn't find any gridObject, check if its on ground.
            if(_colliders.Length == 0){
                if(playerIsDead) return CollisionType.Walkable;
                if(Physics2D.OverlapCircle(position, .2f, Walls)) return CollisionType.Wall;
                if(Physics2D.OverlapCircle(position, .2f, Grounds)) return CollisionType.Walkable;
                return CollisionType.Wall;
            }

            gridObject = FindHighestPriorityObject(_colliders, playerIsDead);
            if(gridObject is null) return CollisionType.Walkable;
            return gridObject.Type;
        }

        private GridObject FindHighestPriorityObject(Collider2D[] colliders, bool playerDead){
            GridObject gridObject = null;

            uint priority = uint.MaxValue;
            //? Checks if the player is dead and finds the ghost interactable grid object with the highest priority.
            if(playerDead){
                foreach(Collider2D col in _colliders){
                    if(!col.TryGetComponent(out GridObject gridObj)) continue;
                    if(!gridObj.GhostInteractable) continue;

                    //? if the last item had higher priority (closer to 0) go to the next obj.
                    if(gridObj.CollisionPriority > priority) continue;
                    
                    //? otherwise, get that object.
                    priority = gridObj.CollisionPriority;
                    gridObject = gridObj;
                }

                return gridObject;
            }

            //? Finds grid object with the highest priority.
            foreach(Collider2D col in _colliders){
                if(!col.TryGetComponent(out GridObject gridObj)) continue;
                
                //? if the last item had higher priority (closer to 0) go to the next obj.
                if(gridObj.CollisionPriority > priority) continue;
                
                //? otherwise, get that object.
                priority = gridObj.CollisionPriority;
                gridObject = gridObj;
            }
            if(gridObject is not null) return gridObject;
            return null;
        }

    }
}