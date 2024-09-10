using System;
using Reggborn.Core;
using Reggborn.Player;
using UnityEngine;
namespace Reggborn.Objects
{
    public abstract class GridObject : MonoBehaviour
    {
        /// <summary>
        /// The event called when an interaction is successfull, Eg: Pushing a <see cref="MovableRock"/> away.
        /// </summary>
        public static event Action OnInteractionSuccessful;

        /// <summary>
        /// The priority of the collision, if it's lower it will be detected over other objects.
        /// </summary>
        public abstract uint CollisionPriority { get; protected set; }
        /// <summary>
        /// The CollisionType of the object.
        /// </summary>
        public abstract CollisionType Type { get; protected set; }
        /// <summary>
        /// Defines if it will be interactable as a ghost.
        /// </summary>
        public abstract bool GhostInteractable { get; protected set; }

        protected virtual void Start()
        {
            TickManager.OnTick += OnTickEvent;
        }

        protected virtual void OnDisable()
        {
            TickManager.OnTick -= OnTickEvent;
        }

        protected virtual void OnTickEvent(int tick) { }
        public abstract void Interact(PlayerController player);

        protected virtual void InteractionSuccessful()
        {
            OnInteractionSuccessful?.Invoke();
        }

        protected virtual void OnDrawGizmos()
        {
            if (Application.isPlaying) return;
            transform.position = Vector3Int.RoundToInt(transform.position);
        }
    }
}