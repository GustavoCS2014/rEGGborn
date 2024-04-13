using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Interfaces;
using Player;
using UnityEngine;

public class Spikes : MonoBehaviour, IDamager
{
    [SerializeField] private bool spikesOut;
    [SerializeField] private int timeOut;
    [SerializeField] private int timeIn;
    [SerializeField] private Transform SpikesUp;
    [SerializeField] private Transform SpikesDown;

    private int _counter;

    private void Start() {
        GameManager.OnMovesIncreased += OnMovesIncreasedEvent;

        SpikesUp.gameObject.SetActive(spikesOut);
        SpikesDown.gameObject.SetActive(!spikesOut);
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
        SpikesDown.gameObject.SetActive(!spikesOut);
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
}
