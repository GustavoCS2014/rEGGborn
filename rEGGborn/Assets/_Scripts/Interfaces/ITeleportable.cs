using UnityEngine;

namespace Interfaces{
    public interface ITeleportable{
        Vector3 GetPosition();
        GameObject GetGameObject();
    }
}