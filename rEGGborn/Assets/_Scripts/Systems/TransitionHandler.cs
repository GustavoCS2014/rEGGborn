using UnityEngine;
using Utilities;

public class TransitionHandler : MonoBehaviour {

    [SerializeField] private SceneField targetScene;

    public void ChangeScene(){
        SceneTransitioner.Instance.LoadAndChangeScene(targetScene);
    }

}