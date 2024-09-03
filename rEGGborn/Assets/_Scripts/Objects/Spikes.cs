using System;
using Player;
using UnityEngine;

namespace Objects
{
    public class Spikes : GridObject
    {
        public static event Action OnSpikesUp;
        public override uint CollisionPriority { get; protected set; } = 2;
        public override CollisionType Type { get; protected set; } = CollisionType.Walkable;
        public override bool GhostInteractable { get; protected set; } = false;
        [SerializeField] private bool spikesUp;
        [SerializeField] private Transform SpikesUp;
        [SerializeField] private NewCollisionManager collisionManager;
        private int _order;

        protected override void Start()
        {
            base.Start();
            SpikesUp.gameObject.SetActive(spikesUp);

            PlayerController.OnSuccessfulAction += OnSuccessfulActionEvent;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            PlayerController.OnSuccessfulAction -= OnSuccessfulActionEvent;
        }

        private void OnSuccessfulActionEvent()
        {
            spikesUp = !spikesUp;
            SpikesUp.gameObject.SetActive(spikesUp);
        }
        public override void Interact(PlayerController player)
        {
            if (spikesUp)
                player.Die();
        }

        public GameObject GetGameObject() => gameObject;
        public Transform GetTransform() => transform;
    }
}
