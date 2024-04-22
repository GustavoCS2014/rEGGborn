using Attributes;
using Core;
using Player;
using UnityEngine;
namespace System{
    public class TickManager : MonoBehaviour{
        public static TickManager Instance {get; private set;}

        public static event Action<int> OnTick;
        [SerializeField] private float tickTime;
        [SerializeField, ReadOnly] public int TickCount;
        [SerializeField, ReadOnly] public bool PlayerActed; 
        private float _timer;

        private void Awake() {
            if(Instance is not null){
                Destroy(gameObject);
            }else{
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start() {
            PlayerController.OnSuccessfulInput += OnSuccessfulInputEvent;
        }


        private void OnDestroy(){
            PlayerController.OnSuccessfulInput -= OnSuccessfulInputEvent;
        }

        private void Update(){
            if(!PlayerActed) return;
            if(_timer > tickTime){
                _timer = 0;
                TickCount++;
                OnTick?.Invoke(TickCount);
                PlayerActed = false;
                GameManager.Instance.ChangeState(GameState.Playing);
            }
            _timer += Time.deltaTime;
        }

        private void OnSuccessfulInputEvent(){
            PlayerActed = true;
            GameManager.Instance.ChangeState(GameState.TickCooldown);
        }

    }
}