using UnityEngine;
[RequireComponent(typeof(Camera))]
public sealed class PermanentCamera : MonoBehaviour
{
    public static PermanentCamera Instance { get; private set; }
    public Camera Cam;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Cam = GetComponent<Camera>();
        Instance = this;
        DontDestroyOnLoad(this);
    }
}