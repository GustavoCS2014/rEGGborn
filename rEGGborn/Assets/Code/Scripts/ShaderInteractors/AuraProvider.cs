using EditorAttributes;
using UnityEngine;
namespace Reggborn.Shader
{
    public sealed class AuraProvider : MonoBehaviour
    {
        [SerializeField, ReadOnly] private Material Aura;
        [SerializeField, ReadOnly] private Vector2 direction;
        private Vector2 _lastPos;
        private Vector2 _lastDir;

        private void Awake()
        {
            Aura = GetComponent<SpriteRenderer>().material;
            _lastDir = Vector2.up;
            direction = _lastDir;
        }

        private void FixedUpdate()
        {
            _lastDir = direction;

            direction = Vector2.Lerp(_lastDir, (_lastPos - (Vector2)transform.position).normalized, Time.fixedDeltaTime * 1.5f);
            direction = Vector2.Lerp(direction, Vector2.up, Time.fixedDeltaTime * .5f);
            _lastPos = transform.position;

            // if (direction.magnitude < 0.005f)
            // {
            //     direction = Vector2.up;
            // }
            Aura.SetVector("_direction", new Vector4(direction.x, direction.y, 0, 0));
        }
    }
}