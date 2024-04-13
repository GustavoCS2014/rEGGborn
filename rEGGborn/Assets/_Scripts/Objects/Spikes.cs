using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Player;
using UnityEngine;

public class Spikes : MonoBehaviour, IDamager
{
    public void Damage(PlayerController player)
    {
        player.Die();
    }
}
