using Reggborn.Inputs;
using Reggborn.UI;
using UnityEngine;

public class CreditsUI : MonoBehaviour, IUserInterface
{
    private void OnEnable()
    {
        InputManager.OnAnyInput += OnAnyInputEvent;
    }

    private void OnDisable()
    {
        InputManager.OnAnyInput -= OnAnyInputEvent;
    }

    private void OnAnyInputEvent(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.canceled) return;
        Close();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
