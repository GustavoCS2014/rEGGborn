using UnityEngine;
[RequireComponent(typeof(Camera))]
public sealed class PermanentCamera : MonoBehaviour
{
    public static PermanentCamera Instance { get; private set; }
    public Camera Cam;


    private void Awake()
    {
        Cam = GetComponent<Camera>();
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }
}