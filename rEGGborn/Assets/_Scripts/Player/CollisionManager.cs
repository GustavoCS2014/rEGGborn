using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Player;
using UnityEngine;

public class CollisionManager : MonoBehaviour {
    public static CollisionManager Instance {get; private set;}

    [Header("LayerMasks"), Space(10)]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private LayerMask interactableMask;

    // private PlayerController _player;

    private void Awake() {
        if(Instance){
            Destroy(gameObject);
        }else{
            Instance = this;
        }
    }

    private void Start() {
        // _player = PlayerController.Instance;
    }
    

    /// <summary>
    /// Checks collisions and returns a <see cref="CollisionType"/>,
    /// and outputs an <see cref="IInteractable"/> if the collision is interactable.
    /// </summary>
    /// <param name="position">The position at which you wanna check collisions.</param>
    /// <param name="playerIsDead">Boolean if the player is dead.</param>
    /// <param name="interactable">Output interactable, use this to detect which type of interactable it is.</param>
    /// <returns></returns>
    public CollisionType CheckCollisionAt(Vector2 position, bool playerIsDead, out IInteractable interactable){
        Collider2D collider = Physics2D.OverlapCircle(position, .4f, interactableMask);
        interactable = null;

        if(playerIsDead){
            if(!collider) return CollisionType.Walkable;
            //? collides with ghostInteractable, return interactable.
            if(collider.TryGetComponent(out IEgg egg)){
                interactable = egg;
                return CollisionType.Walkable;
            }
            return CollisionType.Walkable;
        }
        //? if hit some interactable.
        if(collider){
            if(collider.TryGetComponent(out IMovable movable)){
                interactable = movable;
                return CollisionType.Wall;
            }
            if(collider.TryGetComponent(out IEgg ghostInteractable)){
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
