using Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class TransitionHandler : MonoBehaviour {

    [Tooltip("WARNING!! only set if you want to transition to non level Scene.")]
    [SerializeField] protected SceneSettings targetScene;
    public void SetTargetScene(SceneSettings scene) => targetScene = scene;

    public virtual void ChangeScene(){
        SceneTransitioner.Instance.LoadAndChangeScene(targetScene.Scene);
        GameManager.Instance.SetScene(targetScene);
    }
    public virtual void ChangeSceneNoTransition(){
        SceneTransitioner.Instance.ChangeScene(targetScene.Scene);
        GameManager.Instance.SetScene(targetScene);
    }

    public virtual void ReloadScene(){
        SceneField currentScene = GameManager.Instance.GetCurrentScene().Scene;
        SceneTransitioner.Instance.LoadAndChangeScene(currentScene);
        GameManager.Instance.SetScene(targetScene);
    }
    public virtual void ReloadSceneNoTransition(){
        SceneField currentScene = GameManager.Instance.GetCurrentScene().Scene;
        SceneTransitioner.Instance.ChangeScene(currentScene);
        GameManager.Instance.SetScene(targetScene);
    }

}