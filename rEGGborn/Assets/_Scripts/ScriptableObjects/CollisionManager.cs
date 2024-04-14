using Interfaces;
using UnityEngine;

[CreateAssetMenu(fileName = "CollisionManager", menuName = "Collision Manager")]
public class CollisionManager : ScriptableObject {

    [Header("LayerMasks"), Space(10)]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask LimitMask;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private LayerMask interactableMask;

    /// <summary>
    /// Checks collisions and returns a <see cref="CollisionType"/>,
    /// and outputs an <see cref="IInteractable"/> if the collision is interactable.
    /// </summary>
    /// <param name="position">The position at which you wanna check collisions.</param>
    /// <param name="playerIsDead">Boolean if the player is dead.</param>
    /// <param name="interactable">Output interactable, use this to detect which type of interactable it is.</param>
    /// <returns></returns>
    public CollisionType CheckCollisionAt(Vector2 position, bool playerIsDead, out IInteractable interactable){
        interactable = null;
        if(Physics2D.OverlapCircle(position, .4f, LimitMask)) return CollisionType.Wall;
        Collider2D collider = Physics2D.OverlapCircle(position, .4f, interactableMask);

        if(playerIsDead){
            if(!collider) return CollisionType.Walkable;
            //? collides with ghostInteractable, return interactable.
            if(collider.TryGetComponent(out IEgg egg)){
                interactable = egg;
                return CollisionType.Walkable;
            }
            if(collider.TryGetComponent(out IDamager damager)){
                interactable = damager;
                return CollisionType.Walkable;
            }
            return CollisionType.Walkable;
        }
        //? if hit some interactable.
        if(collider){
            if(collider.TryGetComponent(out IGoal goal)){
                goal.WinStage();
                return CollisionType.Walkable;
            }
            if(collider.TryGetComponent(out IMovable movable)){
                interactable = movable;
                return CollisionType.Wall;
            }
            if(collider.TryGetComponent(out IEgg egg)){
                return CollisionType.Wall;
            }
            if(collider.TryGetComponent(out IDamager damager)){
                interactable = damager;
                return CollisionType.Walkable;
            }
        }

        if(Physics2D.OverlapCircle(position, .4f, wallMask)) return CollisionType.Wall;
        if(Physics2D.OverlapCircle(position, .4f, groundMask)) return CollisionType.Walkable;
        return CollisionType.Wall;
    }

}
