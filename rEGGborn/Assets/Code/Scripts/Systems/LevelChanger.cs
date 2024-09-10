namespace Reggborn.Core
{
    public class LevelChanger : TransitionHandler
    {
        public void LoadLevel()
        {
            SetTargetScene(targetScene);
            base.ChangeScene();
            GameManager.Instance.ResetMoveCount();
        }

        public void ResetLevel()
        {
            SetTargetScene(GameManager.Instance.GetCurrentScene());
            base.ReloadScene();
            GameManager.Instance.ResetMoveCountWhitoutStoring();

        }
    }
}
