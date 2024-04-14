using UnityEngine;

public class LevelChanger : TransitionHandler
{
    public void NextLevel()
    {
        base.ChangeScene();
        GameManager.Instance.ResetMoveCount();
        GameManager.Instance.ChangeState(GameStates.Playing);
    }

    public void ResetLevel(){
        base.ReloadScene();
        GameManager.Instance.ResetMoveCountWhitoutStoring();
        GameManager.Instance.ChangeState(GameStates.Playing);
        
    }
}
