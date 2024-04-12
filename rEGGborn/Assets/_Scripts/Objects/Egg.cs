using Interfaces;
using UnityEngine;


public class Egg : MonoBehaviour, IGhostInteractable{
    public void Reborn(out Vector3 position){
        position = transform.position;
        Destroy(gameObject);
    }
}
