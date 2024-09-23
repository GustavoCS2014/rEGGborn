using EditorAttributes;
using Reggborn.Core;
using Reggborn.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Reggborn.Objects
{
    public class Spikes : GridObject
    {
        public static UnityAction<AudioClip, Vector3> PlayUpSound;
        [SerializeField] private AudioClip SpikesUpSound;
        [SerializeField] private AudioClip SpikesDownSound;

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

            if (spikesUp)
            {
                PlayUpSound?.Invoke(SpikesUpSound, transform.position);
                PlayAnimation(upAnimation.name);
                return;
            }

            PlayUpSound?.Invoke(SpikesDownSound, transform.position);
            PlayAnimation(downAnimation.name);
            // return;

            // PlayAnimation(spikesUp ? "SpikesUp" : "SpikesDown");
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
