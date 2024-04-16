using Core;
using Interfaces;
using Player;
using UnityEngine;

namespace Objects{
    public class Spikes : MonoBehaviour, IDamager
    {
        [SerializeField] private bool spikesOut;
        [SerializeField] private int timeOut;
        [SerializeField] private int timeIn;
        [SerializeField] private Transform SpikesUp;
        [SerializeField] private LayerMask stopColliderMask;

        private int _counter;

        private void Start() {
            GameManager.OnMovesIncreased += OnMovesIncreasedEvent;

            SpikesUp.gameObject.SetActive(spikesOut);
        }

        private void OnDisable() {
            
            GameManager.OnMovesIncreased -= OnMovesIncreasedEvent;
        }

        private void Update(){
            float checkSize = .2f;
            if(Physics2D.OverlapCircle(transform.position, checkSize, stopColliderMask)) {
                GetComponent<Collider2D>().enabled = false;
                return;
            }
            GetComponent<Collider2D>().enabled = true;
            
        }

        public GameObject GetGameObject() => gameObject;

        public Transform GetTransform() => transform;

        private void OnMovesIncreasedEvent(int moves){
            if(spikesOut){
                _counter++;
                if(_counter < timeOut) return;
                _counter = 0;
                spikesOut = !spikesOut;
            }else{
                _counter++;
                if(_counter < timeIn) return;
                _counter = 0;
                spikesOut = !spikesOut;
            }
            SpikesUp.gameObject.SetActive(spikesOut);

        }

        public void Damage(PlayerController player)
        {
            //! Bc of the way the collisions are detected, we damage the player if the Spikes are hidden. xD
            if(spikesOut)   
                player.Die();
        }

        private void OnDrawGizmos() {
            Vector3Int pos = Vector3Int.RoundToInt(transform.position);
            transform.position = pos;
        }

        public bool IsDamaging() => spikesOut;
    }
}
