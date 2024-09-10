using Reggborn.Core;
using EditorAttributes;
using Reggborn.Player;
using UnityEngine;

namespace Reggborn.Objects
{
    public class Egg : GridObject
    {
        public override uint CollisionPriority { get; protected set; } = 3;
        public override CollisionType Type { get; protected set; } = CollisionType.Walkable;
        public override bool GhostInteractable { get; protected set; } = true;
        [SerializeField, ReadOnly] private bool hasTeleported;

        public static bool CanBeLaid(Vector3 layPosition, bool playerIsGhost, NewCollisionManager manager)
        {

            CollisionType collision = manager.DetectCollisionAt(
                layPosition,
                playerIsGhost,
                out GridObject bodyObj
            );

            //? if there's something other than a portal on this tile, can't lay the egg. 
            if (bodyObj is not null && bodyObj is not Portal) return false;
            return true;
        }

        public override void Interact(PlayerController player)
        {
            if (player.IsAlive()) return;

            player.Revive();
            Destroy(gameObject);
        }

        public void BreakEgg()
        {

        }

        public Transform GetTransform() => transform;
        public GameObject GetGameObject() => gameObject;
    }
}