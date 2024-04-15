using Attributes;
using Core;
using Interfaces;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Objects{
    public class Egg : MonoBehaviour, IEgg, ITeleportable {
        [SerializeField] private LayerMask DetectionLayer;
        [SerializeField, ReadOnly] private bool hasTeleported;

        private void Start() {
            GameManager.OnMovesIncreased += OnMovesIncreasedEvent;
        }

        private void OnDisable() {
            GameManager.OnMovesIncreased -= OnMovesIncreasedEvent;
        }


        private void OnMovesIncreasedEvent(int obj)
        {
            CheckCollision();
        }

        public GameObject GetGameObject() => gameObject;

        public Vector3 GetPosition() => transform.position;

        public Transform GetTransform() => transform;

        public void Hatch(PlayerController player){
            player.Revive();
            Destroy(gameObject);
        }

        private void CheckCollision(){
            if(hasTeleported) return;
            Collider2D collision;
            float checkSize = .2f;
            if(!(collision = Physics2D.OverlapCircle(transform.position, checkSize, DetectionLayer))) return;
            if(collision.TryGetComponent(out IPortal portal)){
                if(!portal.IsSender()){
                    return;
                }
                portal.Teleport(this);
                hasTeleported = true;
            }
        }
    }
}