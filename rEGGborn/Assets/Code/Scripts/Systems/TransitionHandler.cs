using System;
using Reggborn.Core;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    public static event Action<GameState> OnSceneChanged;

    [Tooltip("WARNING!! only set if you want to transition to non level Scene.")]
    [SerializeField] protected SceneSettings targetScene;
    public void SetTargetScene(SceneSettings scene) => targetScene = scene;

    private void Start()
    {
        SceneTransitioner.OnFadeInEnded += OnFadeInEndedEvent;
    }

    private void OnDisable()
    {
        SceneTransitioner.OnFadeInEnded -= OnFadeInEndedEvent;
    }

    private void OnFadeInEndedEvent()
    {
        OnSceneChanged?.Invoke(GameManager.Instance.State);
    }

    public virtual void ChangeScene()
    {
        if (!targetScene)
        {
            Debug.LogWarning($"NO TARGET SCENE HAS BEEN ASIGNED.");
            return;
        }

        SceneTransitioner.Instance.LoadAndChangeScene(targetScene.Scene);
        GameManager.Instance.SetScene(targetScene);
    }
    public virtual void ChangeSceneNoTransition()
    {
        if (!targetScene)
        {
            Debug.LogWarning($"NO TARGET SCENE HAS BEEN ASIGNED.");
            return;
        }

        SceneTransitioner.Instance.ChangeScene(targetScene.Scene);
        GameManager.Instance.SetScene(targetScene);
        OnSceneChanged?.Invoke(targetScene.StartingState);
    }

    public virtual void ReloadScene()
    {
        SceneSettings currentScene = GameManager.Instance.GetCurrentScene();
        SceneTransitioner.Instance.LoadAndChangeScene(currentScene.Scene);
        GameManager.Instance.SetScene(currentScene);
    }
    public virtual void ReloadSceneNoTransition()
    {
        SceneSettings currentScene = GameManager.Instance.GetCurrentScene();
        SceneTransitioner.Instance.ChangeScene(currentScene.Scene);
        GameManager.Instance.SetScene(currentScene);
        OnSceneChanged?.Invoke(currentScene.StartingState);
    }

}