using Attributes;
using Core;
using Player;
using UnityEngine;
namespace System
{
    public class TickManager : MonoBehaviour
    {
        public static TickManager Instance { get; private set; }

        public static event Action<int> OnTick;
        [SerializeField] private float baseTickDuration;
        public float BaseTickDuration => baseTickDuration;
        [field: SerializeField, ReadOnly] public float TickDuration { get; private set; }
        [SerializeField, ReadOnly] public int TickCount;
        [SerializeField, ReadOnly] public bool PlayerActed;
        private float _timer;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            TickDuration = baseTickDuration;
        }

        private void Start()
        {
            PlayerController.OnSuccessfulAction += OnSuccessfulInputEvent;
        }


        private void OnDisable()
        {
            PlayerController.OnSuccessfulAction -= OnSuccessfulInputEvent;
        }

        private void Update()
        {
            if (!PlayerActed) return;
            if (_timer > TickDuration)
            {
                _timer = 0;
                TickCount++;
                OnTick?.Invoke(TickCount);
                PlayerActed = false;
                GameManager.Instance.ChangeState(GameState.Playing);
            }
            _timer += Time.deltaTime;
        }

        private void OnSuccessfulInputEvent()
        {
            PlayerActed = true;
            GameManager.Instance.ChangeState(GameState.TickCooldown);
        }

        public void ChangeTickDuration(float duration)
        {
            if (TickDuration != duration)
                TickDuration = duration;
        }

        public void ResetTickDuration()
        {
            if (TickDuration != baseTickDuration)
                TickDuration = baseTickDuration;
        }
    }
}