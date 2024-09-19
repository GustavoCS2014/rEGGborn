using Reggborn.Core;
using EditorAttributes;
using Reggborn.Player;
using UnityEngine;

namespace Reggborn.Objects
{
    public class Egg : GridObject
    {
        [SerializeField] private EggVisuals visuals;
        public override uint CollisionPriority { get; protected set; } = 3;
        public override CollisionType Type { get; protected set; } = CollisionType.Walkable;
        public override bool GhostInteractable { get; protected set; } = true;
        [field: SerializeField, ReadOnly] public bool HasTeleported { get; private set; }

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

        public void Teleport(Vector3 targetDir)
        {
            visuals.EnterPortal(targetDir);
            HasTeleported = true;
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }
        public Transform GetTransform() => transform;
        public GameObject GetGameObject() => gameObject;
    }
}