using UnityEngine;

public class CameraAspectRatioSetter : MonoBehaviour
{
    [SerializeField] private Vector2 resTarget;

    private void Update()
    {
        Vector2 resViewport = new Vector2(Screen.width, Screen.height);
        Vector2 resNormalized = resTarget / resViewport; // target res in viewport space
        Vector2 size = resNormalized / Mathf.Max(resNormalized.x, resNormalized.y);
        Camera.main.rect = new Rect(default, size) { center = new Vector2(0.5f, 0.5f) };
    }
}