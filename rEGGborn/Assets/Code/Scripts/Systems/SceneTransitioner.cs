using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reggborn.Core
{
    public class SceneTransitioner : MonoBehaviour
    {

        public static SceneTransitioner Instance { get; private set; }
        public static event Action OnFadeInEnded;

        const string FADE_IN = "FADE_IN";
        const string FADE_OUT = "FADE_OUT";
        [SerializeField] private Animator transitionAnimator;
        private AsyncOperation _transition;
        private bool _fadeInEnded;
        private GameState _targetSceneStartingGameState;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        private void Update()
        {
            // if(_transition is not null) Debug.Log($"{_transition.progress * 100}%");
            if (!_fadeInEnded) return;
            if (_transition.progress > .85f)
            {
                transitionAnimator.SetTrigger(FADE_OUT);
                _transition.allowSceneActivation = true;
                _fadeInEnded = false;
            }
        }

        public void EndedFadeIn()
        {
            _fadeInEnded = true;
            OnFadeInEnded?.Invoke();
        }

        /// <summary>
        /// Loads a scene with transition.
        /// </summary>
        /// <param name="targetScene">Serializable Scene</param>
        public void LoadAndChangeScene(SceneField targetScene)
        {
            _transition = SceneManager.LoadSceneAsync(targetScene);
            transitionAnimator.SetTrigger(FADE_IN);
            _transition.allowSceneActivation = false;
        }

        /// <summary>
        /// Loads a scene with transition.
        /// </summary>
        /// <param name="targetScene">Scene name</param>
        public void LoadAndChangeScene(string targetScene)
        {
            _transition = SceneManager.LoadSceneAsync(targetScene);
        }

        /// <summary>
        /// Changes the scene without transition.
        /// </summary>
        /// <param name="targetScene">Serializable Scene</param>
        public void ChangeScene(SceneField targetScene)
        {
            SceneManager.LoadScene(targetScene);
        }

        /// <summary>
        /// Changes the scene without transition.
        /// </summary>
        /// <param name="targetScene">Scene name</param>
        public void ChangeScene(string targetScene)
        {
            SceneManager.LoadScene(targetScene);
        }

    }
}
