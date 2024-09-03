using Player;
using UnityEngine;
using UnityEngine.Events;
namespace Utilities
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTriggerEnter;
        [SerializeField] private UnityEvent onTriggerStay;
        [SerializeField] private UnityEvent onTriggerExit;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerController player)) return;
            onTriggerEnter?.Invoke();
        }

        private void OnTriggerStay2D(Collider2D other)
        {

            if (!other.TryGetComponent(out PlayerController player)) return;
            onTriggerStay?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {

            if (!other.TryGetComponent(out PlayerController player)) return;
            onTriggerExit?.Invoke();
        }
    }
}