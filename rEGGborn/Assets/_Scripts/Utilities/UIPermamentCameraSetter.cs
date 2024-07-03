using UnityEngine;
[RequireComponent(typeof(Canvas))]
public sealed class UIPermamentCameraSetter : MonoBehaviour
{
    private Canvas UI;
    private void Awake()
    {
        UI = GetComponent<Canvas>();
    }

    private void Start()
    {
        UI.worldCamera = PermanentCamera.Instance.Cam;
    }
}