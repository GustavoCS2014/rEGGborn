using System;
using Objects.Settings;
using Player;
using UnityEngine;
namespace Objects{
    public sealed class Portal : GridObject
    {
        public static event Action<uint, GridObject> OnTeleport;
        
        public override uint CollisionPriority { get; protected set; } = 4;
        public override CollisionType Type { get; protected set; } = CollisionType.Walkable;
        public override bool GhostInteractable { get; protected set; } = false;

        [SerializeField] private NewCollisionManager collisionManager;
        [SerializeField] private bool sender;
        [SerializeField] private PortalLink portalLink;

        protected override void Start()
        {
            base.Start();
            OnTeleport += OnTeleportEvent;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnTeleport -= OnTeleportEvent;
        }

        private void OnTeleportEvent(uint LinkKey, GridObject Subject)
        {
            if(sender) return;
            if(LinkKey != portalLink.LinkKey) return;

            Subject.transform.position = transform.position;
        }

        protected override void OnTickEvent(int ticks){
            collisionManager.DetectCollisionAt(transform.position, out GridObject gridObject);
            if(gridObject is Egg){
                OnTeleport?.Invoke(portalLink.LinkKey, gridObject);
            }
        }

        public override void Interact(PlayerController player) {}

    }
}
