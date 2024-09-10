using Reggborn.Core;
using Reggborn.Player;
using UnityEngine;

namespace Reggborn.Objects
{
    public sealed class MovableRock : GridObject
    {
        public override uint CollisionPriority { get; protected set; } = 1;
        public override CollisionType Type { get; protected set; } = CollisionType.Wall;
        public override bool GhostInteractable { get; protected set; } = false;

        [SerializeField] private NewCollisionManager collisionManager;

        protected override void Start()
        {
            base.Start();
        }

        public override void Interact(PlayerController player)
        {
            Vector3 direction = transform.position - player.transform.position;
            if (VerifyMove(transform.position + direction))
            {
                // transform.position += Vector3Int.RoundToInt(direction);
                Vector3 intPos = Vector3Int.RoundToInt(transform.position + direction);
                // transform.DOMove(intPos, TickManager.Instance.TickDuration).SetEase(Ease.OutExpo);
                transform.position += direction;
                base.InteractionSuccessful();
            }

        }

        public bool VerifyMove(Vector2 pushPosition)
        {
            CollisionType type = collisionManager.DetectCollisionAt(pushPosition, out GridObject gridObject);

            if (gridObject is MovableRock)
            {
                FailedAction(pushPosition - (Vector2)transform.position);
                return false;
            }

            if (type is CollisionType.Wall)
            {
                FailedAction(pushPosition - (Vector2)transform.position);
                return false;
            }

            if (gridObject is Spikes) return true;
            if (gridObject is Portal) return true;
            if (gridObject is null) return true;


            FailedAction(pushPosition - (Vector2)transform.position);
            return false;
        }

        private void FailedAction(Vector2 pushDirection)
        {
            float duration = TickManager.Instance.TickDuration;
            Vector2 direction = pushDirection * .2f;
            // transform.DOPunchPosition(direction, duration, 10, .5f).SetEase(Ease.InOutCubic);
            // transform.DOShakeRotation(duration, 20, 10).SetEase(Ease.InSine);
            base.InteractionSuccessful();
        }

        public Transform GetTransform() => transform;
        public GameObject GetGameObject() => gameObject;
    }
}