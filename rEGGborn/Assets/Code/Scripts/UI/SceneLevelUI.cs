using Reggborn.Core;
using UnityEngine;
using UnityEngine.UI;
namespace Reggborn.UI
{
    public abstract class SceneLevelUI : MonoBehaviour, IUserInterface
    {
        [SerializeField] protected Button defaultButton;
        [SerializeField] protected Button restartBTN;
        [SerializeField] protected Button mainMenuBTN;
        [SerializeField] protected SceneSettings mainMenuScene;

        public virtual void Close()
        {
            restartBTN.onClick.RemoveAllListeners();
            mainMenuBTN.onClick.RemoveAllListeners();
        }

        public virtual void Show()
        {
            restartBTN.onClick.AddListener(ReloadLevel);
            mainMenuBTN.onClick.AddListener(LoadMainMenu);
        }

        protected virtual void ReloadLevel()
        {
            GameManager.Instance.ResetMoveCountWhitoutStoring();
            GameManager.Instance.TransitionHandler.ReloadScene();
        }

        protected virtual void LoadMainMenu()
        {
            GameManager.Instance.TransitionHandler.SetTargetScene(mainMenuScene);
            GameManager.Instance.TransitionHandler.ChangeScene();
        }
    }
}