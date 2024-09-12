using UnityEngine;

public class CameraCrearFlags : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private void Update()
    {
        cam.clearFlags = CameraClearFlags.SolidColor;
    }
}