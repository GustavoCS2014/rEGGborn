using Interfaces;
using Player;
using UnityEngine;

namespace Objects{
    public class Egg : MonoBehaviour, IEgg{
        public GameObject GetGameObject() => gameObject;

        public Transform GetTransform() => transform;

        public void Hatch(PlayerController player){
            player.Revive();
            Destroy(gameObject);
        }
    }
}