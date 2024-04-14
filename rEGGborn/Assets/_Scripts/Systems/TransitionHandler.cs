using UnityEngine;
using Utilities;

public class TransitionHandler : MonoBehaviour {

    [SerializeField] private SceneField targetScene;

    public virtual void ChangeScene(){
        SceneTransitioner.Instance.LoadAndChangeScene(targetScene);
    }
    public virtual void ChangeSceneNoTransition(){
        SceneTransitioner.Instance.ChangeScene(targetScene);
    }

}