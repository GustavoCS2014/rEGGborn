using UnityEngine;

public class LevelChanger : TransitionHandler
{
    public override void ChangeScene()
    {
        base.ChangeScene();
        GameManager.Instance.ResetMoveCount();
        GameManager.Instance.ChangeState(GameStates.Playing);
    }
}
