using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Interfaces{
    public interface IEgg : IInteractable {
        void Hatch(PlayerController player);
    }
}
