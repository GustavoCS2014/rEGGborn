using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class TransitionHandler : MonoBehaviour {

    [SerializeField] private SceneField targetScene;

    public virtual void ChangeScene(){
        SceneTransitioner.Instance.LoadAndChangeScene(targetScene);
    }
    public virtual void ChangeSceneNoTransition(){
        SceneTransitioner.Instance.ChangeScene(targetScene);
    }
    
    public virtual void ReloadScene(){
        string currentScene = SceneManager.GetActiveScene().name;
        SceneTransitioner.Instance.LoadAndChangeScene(currentScene);
    }
    public virtual void ReloadSceneNoTransition(){
        string currentScene = SceneManager.GetActiveScene().name;
        SceneTransitioner.Instance.ChangeScene(currentScene);
    }

}