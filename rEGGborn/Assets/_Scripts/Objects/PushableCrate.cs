using Interfaces;
using UnityEngine;

public class PushableCrate : MonoBehaviour, IMovable
{
    [SerializeField] private LayerMask unpushableMask;
    [SerializeField] private LayerMask pushableSurfaceMask;
    private Vector3 _nextPos;

    private void OnValidate() {
        // Vector3Int pos = Vector3Int.FloorToInt(transform.position);
        // Debug.Log($"intpos {pos}");
        // transform.position = pos;
    }

    private void OnDrawGizmos() {
        Vector3Int pos = Vector3Int.RoundToInt(transform.position);
        transform.position = pos;
        Gizmos.color = Color.red;
        if(CanBePushedTo(_nextPos))
            Gizmos.color = Color.green;
        Gizmos.DrawSphere(_nextPos, .2f);
    }

    public GameObject GetGameObject() => gameObject;

    public Transform GetTransform() => transform;

    public void PushTo(Vector2 direction)
    {
        _nextPos = (Vector2)transform.position + direction;
        if(!CanBePushedTo(_nextPos)) return;
        transform.position += (Vector3)direction;
    }

    public bool CanBePushedTo(Vector2 position){
        float checkSize = .2f;
        if(Physics2D.OverlapCircle(position, checkSize, unpushableMask)) return false;
        if(Physics2D.OverlapCircle(position, checkSize, pushableSurfaceMask))return true;
        return false;
    }

}
