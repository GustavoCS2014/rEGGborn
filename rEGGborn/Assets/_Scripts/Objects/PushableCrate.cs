using Interfaces;
using UnityEngine;

public class PushableCrate : MonoBehaviour, IMovable
{
    [SerializeField] private LayerMask pushableSurfaceMask;
    private Vector3 _nextPos;

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + _nextPos, .2f);
    }

    public void Push(Vector2 direction)
    {
        _nextPos = direction;
        transform.position += (Vector3)direction;
    }

    public bool CanBePushed(Vector2 direction){
        if(!Physics2D.OverlapBox((Vector2)transform.position + direction, Vector2.one * .5f, 0f, pushableSurfaceMask)){
            return false;
        }
        return true;
    }
}
