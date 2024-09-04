using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class SetLevelCameraUI : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        if (!cam) cam = Camera.main;

        canvas.worldCamera = cam;
    }
}