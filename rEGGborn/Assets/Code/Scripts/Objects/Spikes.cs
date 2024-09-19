using EditorAttributes;
using Reggborn.Core;
using Reggborn.Player;
using UnityEngine;

namespace Reggborn.Objects
{
    public class Spikes : GridObject
    {
        public override uint CollisionPriority { get; protected set; } = 2;
        public override CollisionType Type { get; protected set; } = CollisionType.Walkable;
        public override bool GhostInteractable { get; protected set; } = false;
        [SerializeField] private bool spikesUp;
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationClip upAnimation;
        [SerializeField] private AnimationClip downAnimation;
        [SerializeField] private NewCollisionManager collisionManager;

        protected override void Start()
        {
            base.Start();
            PlayerController.OnSuccessfulAction += OnSuccessfulActionEvent;
            PlayAnimation(spikesUp ? upAnimation.name : downAnimation.name);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            PlayerController.OnSuccessfulAction -= OnSuccessfulActionEvent;
        }

        private void OnSuccessfulActionEvent()
        {
            spikesUp = !spikesUp;

            PlayAnimation(spikesUp ? "SpikesUp" : "SpikesDown");
        }
        public override void Interact(PlayerController player)
        {
            if (spikesUp)
                player.Die();
        }

        public GameObject GetGameObject() => gameObject;
        public Transform GetTransform() => transform;

        public void PlayAnimation(string animHash)
        {
            // if (animHash == -1)
            // {
            //     Debug.LogWarning("The animation has not been assigned!");
            //     return;
            // }
            animator.Play(animHash);
        }
    }
}
