using System;
using UnityEngine;
namespace Reggborn.Objects
{
    public sealed class EggVisuals : MonoBehaviour
    {
        [SerializeField] private Egg parent;
        [SerializeField] private Animator eggAnimator;
        [SerializeField] private AnimationClip enterPortalAnim;
        [SerializeField] private AnimationClip exitPortalAnim;
        [SerializeField] private AnimationClip breakEggAnim;

        private Vector3? _destinationPortal;
        private Action _breakCallback;

        public void EnterPortal(Vector3 portalExit)
        {
            _destinationPortal = portalExit;
            if (_destinationPortal is null)
            {
                Debug.LogWarning($"Portal Exit is null");
                return;
            }
            eggAnimator.CrossFade(enterPortalAnim.name, 0);
        }

        //called from animator.
        public void ExitPortal()
        {
            if (_destinationPortal is null) return;
            parent.SetPosition(_destinationPortal.Value);
            eggAnimator.CrossFade(exitPortalAnim.name, 0);
            _destinationPortal = null;
        }

        public void BreakEgg(Action callback)
        {
            _breakCallback = callback;
            eggAnimator.CrossFade(breakEggAnim.name, 0);
        }

        public void PlayBreakCallback()
        {
            _breakCallback?.Invoke();
        }

        public void DestroyEgg()
        {
            Destroy(parent.gameObject);
        }

    }
}