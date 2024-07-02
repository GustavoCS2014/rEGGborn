using System;
using Attributes;
using Core;
using DG.Tweening;
using Inputs;
using Objects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

namespace Player
{
    public sealed class PlayerController : MonoBehaviour
    {

        public static event Action OnPlayerDead;
        public static event Action<int> OnGhostMove;
        public static event Action OnPlayerHatched;
        public static event Action OnShowRespawnHint;
        public static event Action OnSuccessfulAction;

        public static PlayerController Instance { get; private set; }
        public bool EggLaid { get; private set; }
        public PlayerState state;
        [Header("Visuals"), Space(10)]
        [SerializeField] private Transform AliveSprite;
        [SerializeField] private Transform DeadSprite;
        [Header("LayerMasks"), Space(10)]
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private LayerMask wallMask;
        [SerializeField] private LayerMask interactableMask;
        [Header("Egg"), Space(10)]
        [SerializeField] private Egg eggPrefab;
        [SerializeField, ReadOnly] private Egg egg;
        [SerializeField, ReadOnly] private int ghostMoves;
        [SerializeField, ReadOnly] private bool layedEgg;
        private Vector2 _direction;

        private GameManager _gameManager;
        [SerializeField] private NewCollisionManager _newCollisionManager;
        private CollisionType _collisionType;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            InputManager.OnLayEgg += OnLayEggEvent;
            InputManager.OnMovePad += OnMovePadEvent;
            TickManager.OnTick += OnTickEvent;
            GridObject.OnInteractionSuccessful += OnInteractionSuccessfulEvent;
        }

        private void OnDestroy()
        {
            InputManager.OnLayEgg -= OnLayEggEvent;
            InputManager.OnMovePad -= OnMovePadEvent;
            TickManager.OnTick -= OnTickEvent;
            GridObject.OnInteractionSuccessful -= OnInteractionSuccessfulEvent;
        }

        private void Update()
        {
            //?Do things on state.
            switch (state)
            {
                case PlayerState.Alive:
                    ghostMoves = -1;
                    AliveSprite.gameObject.SetActive(true);
                    DeadSprite.gameObject.SetActive(false);
                    break;
                case PlayerState.Ghost:
                    DeadSprite.gameObject.SetActive(true);
                    AliveSprite.gameObject.SetActive(false);
                    //? if the player didn't lay an egg before dying, show a respawn hint.
                    if (!EggLaid)
                    {
                        OnShowRespawnHint?.Invoke();
                    }
                    break;
            }

        }

        #region  EVENTS

        private void OnMovePadEvent(InputAction.CallbackContext context, Vector2 input)
        {
            if (context.canceled) return;
            _direction = input;

            CollisionType collision = _newCollisionManager.DetectCollisionAt(
                (Vector2)transform.position + _direction,
                IsGhost(),
                out GridObject nextObj
            );

            if (nextObj is not null)
            {
                if (nextObj is not Spikes)
                {
                    //? interacting with the object, SuccessfulInput will be called if there's a successful interaction.
                    nextObj.Interact(this);
                }
            }


            //? moving the player if the surface is walkable.
            if (collision is not CollisionType.Walkable) return;
            Move(_direction);
            HandleGhostMoveCount();
            SuccessfulAction();
        }


        //TODO think of a better way to do this, maybe move it to a LayEgg method or smth.
        private void OnLayEggEvent(InputAction.CallbackContext context)
        {
            if (context.canceled) return;
            if (IsGhost()) return;

            if (Egg.CanBeLaid(transform.position, IsGhost(), _newCollisionManager))
            {
                LayEgg();
                SuccessfulAction();
            }
        }

        private void OnTickEvent(int ticks)
        {
            _collisionType = _newCollisionManager.DetectCollisionAt(
                transform.position,
                IsGhost(),
                out GridObject bodyObj
            );

            if (bodyObj is not null)
            {
                bodyObj.Interact(this);
            }
        }

        private void OnInteractionSuccessfulEvent()
        {
            SuccessfulAction();
        }

        #endregion

        #region PLAYER METHODS
        private void Move(Vector3 direction)
        {
            //? Moves the player with a smooth animation.
            // transform.DOMove(transform.position + direction, TickManager.Instance.TickDuration)
            //         .SetEase(Ease.InOutSine);
            float jumpPower = Vector3.Distance(transform.position + direction, transform.position) * 0.3f;

            transform.DOJump(transform.position + direction, jumpPower, 1, TickManager.Instance.TickDuration).SetEase(Ease.InOutSine);
            Sequence JumpScale = DOTween.Sequence();
            JumpScale.Append(
                transform.DOScale(1.05f, TickManager.Instance.TickDuration * 0.5f)
            );

            JumpScale.Append(
                transform.DOScale(1f, TickManager.Instance.TickDuration * 0.5f)
            );



        }

        private void HandleGhostMoveCount()
        {
            if (IsAlive()) return;
            OnGhostMove?.Invoke(ghostMoves);
            ghostMoves--;
            if (ghostMoves < 0) Die();

        }

        private void LayEgg()
        {
            //? if an egg has ben laid, break it and place a new one in the current position.
            if (EggLaid)
            {
                egg.BreakEgg();
                Destroy(egg.gameObject);
                egg = null;
                EggLaid = false;
            }

            egg = Instantiate(eggPrefab, transform.position, quaternion.identity);
            EggLaid = true;
        }

        public void Die()
        {
            if (IsAlive())
            {
                state = PlayerState.Ghost;
                ghostMoves = _gameManager.GetMaxGhostMoves();
                return;
            }
            if (IsGhost())
            {
                OnPlayerDead?.Invoke();
            }
        }

        public void Revive()
        {
            if (IsGhost())
            {
                egg = null;
                EggLaid = false;
                state = PlayerState.Alive;
                OnPlayerHatched?.Invoke();
                return;
            }
        }

        public bool IsAlive() => state is PlayerState.Alive;
        public bool IsGhost() => state is PlayerState.Ghost;

        public void SuccessfulAction() => OnSuccessfulAction?.Invoke();

        #endregion

        #region DEBUG   
        private void OnDrawGizmos()
        {
            Vector3Int pos = Vector3Int.RoundToInt(transform.position);
            transform.position = pos;
        }
        #endregion

    }
}
