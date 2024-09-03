using System;
using Objects;
using UnityEngine;
namespace Player
{
    public sealed class PlayerInteractions : MonoBehaviour
    {
        [SerializeField] private NewCollisionManager newCollisionManager;
        private CollisionType _collisionType;
        private PlayerController _player;

        private void Start()
        {
            _player = PlayerController.Instance;
            TickManager.OnTick += OnTickEvent;
            PlayerController.OnSuccessfulAction += OnSuccessfulInputEvent;
        }


        private void OnDisable()
        {
            TickManager.OnTick -= OnTickEvent;
            PlayerController.OnSuccessfulAction -= OnSuccessfulInputEvent;
        }

        private void OnSuccessfulInputEvent()
        {
        }
        private void OnTickEvent(int obj)
        {

        }
    }
}