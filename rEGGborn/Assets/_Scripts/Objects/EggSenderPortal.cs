using System;
using Core;
using Interfaces;
using Objects.Settings;
using UnityEngine;

namespace Objects{
    public class EggSenderPortal : MonoBehaviour, IPortal
    {
        public static event Action<uint, GameObject> OnTeleport;
        [SerializeField] private PortalLink portalLink;

        public PortalLink PortalLink { 
            get => portalLink; 
            set => portalLink = value;
        }

        private void Start() {
            GameManager.OnMovesIncreased += OnMovesIncresedEvent;
            EggRecieverPortal.OnReverseTeleport += OnReverseTeleportEvent;
        }

        private void OnDisable() {
            GameManager.OnMovesIncreased -= OnMovesIncresedEvent;
            EggRecieverPortal.OnReverseTeleport -= OnReverseTeleportEvent;
            
        }

        private void OnReverseTeleportEvent(uint key, GameObject subject) {
            if(key != PortalLink.LinkKey) return;
            subject.transform.position = transform.position;
        }

        private void OnMovesIncresedEvent(int moves)
        {
            
        }

        public void ReverseTeleport(ITeleportable subject){
            
        }

        public void Teleport(ITeleportable subject){
            OnTeleport?.Invoke(PortalLink.LinkKey, subject.GetGameObject());
        }

        public bool IsSender() => true;

        
        private void OnDrawGizmos() {
            Vector3Int pos = Vector3Int.RoundToInt(transform.position);
            transform.position = pos;
        }
    }
}