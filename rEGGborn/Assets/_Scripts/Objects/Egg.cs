using Interfaces;
using Player;
using UnityEngine;


public class Egg : MonoBehaviour, IGhostInteractable{
    public void Hatch(PlayerController player){
        player.Revive();
        Destroy(gameObject);
    }
}
