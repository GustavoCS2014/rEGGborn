using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Egg : MonoBehaviour {
    

    public void Reborn(out Vector3 position){
        position = transform.position;
        Destroy(gameObject);
    }

}
