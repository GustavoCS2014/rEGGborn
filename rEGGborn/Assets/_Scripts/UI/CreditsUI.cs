using Inputs;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    private void OnEnable() {
        InputManager.OnAnyInput += OnAnyInputEvent;
    }

    private void OnDisable(){
        InputManager.OnAnyInput -= OnAnyInputEvent;
    }

    private void OnAnyInputEvent(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.canceled) return;
        StopCredits();
    }

    public void StopCredits(){
        gameObject.SetActive(false);
    }
}
