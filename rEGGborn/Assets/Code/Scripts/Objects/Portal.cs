using System;
using Reggborn.Settings;
using Reggborn.Player;
using UnityEngine;
using Reggborn.Core;
namespace Reggborn.Objects
{
    public sealed class Portal : GridObject
    {
        public static event Action<uint, Egg> OnTeleport;

        public override uint CollisionPriority { get; protected set; } = 4;
        public override CollisionType Type { get; protected set; } = CollisionType.Walkable;
        public override bool GhostInteractable { get; protected set; } = false;

        [SerializeField] private NewCollisionManager collisionManager;
        [SerializeField] private bool sender;
        [SerializeField] private PortalLink portalLink;
        private bool _cooldown;

        protected override void Start()
        {
            base.Start();
            if (sender) return;
            OnTeleport += OnTeleportEvent;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (sender) return;
            OnTeleport -= OnTeleportEvent;
        }

        private void OnTeleportEvent(uint LinkKey, Egg Subject)
        {
            if (sender) return;
            if (Subject.HasTeleported) return;
            if (LinkKey != portalLink.LinkKey) return;

            Subject.Teleport(transform.position);
        }

        protected override void OnTickEvent(int ticks)
        {
            if (!sender) return;
            collisionManager.DetectCollisionAt(transform.position, out GridObject gridObject);
            if (gridObject is Egg)
            {
                if (_cooldown)
                {
                    _cooldown = false;
                    OnTeleport?.Invoke(portalLink.LinkKey, gridObject as Egg);
                    return;
                }
                _cooldown = true;
            }
        }

        public override void Interact(PlayerController player) { }

    }
}
