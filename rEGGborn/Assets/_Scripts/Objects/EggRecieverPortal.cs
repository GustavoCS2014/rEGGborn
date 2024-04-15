using System;
using Core;
using Interfaces;
using Objects.Settings;
using UnityEngine;

namespace Objects{
    public class EggRecieverPortal : MonoBehaviour, IPortal
    {
        public static event Action<uint, GameObject> OnReverseTeleport;
        [SerializeField] private PortalLink portalLink;

        public PortalLink PortalLink { 
            get => portalLink; 
            set => portalLink = value;
        }

        private void Start() {
            GameManager.OnMovesIncreased += OnMovesIncresedEvent;
            EggSenderPortal.OnTeleport += OnTeleportEvent;
        }
        private void OnDisable() {
            GameManager.OnMovesIncreased -= OnMovesIncresedEvent;
            EggSenderPortal.OnTeleport -= OnTeleportEvent;
        }


        //? if the subject of teleportation has the same key as me, teleport it here.
        private void OnTeleportEvent(uint key, GameObject subject) {
            if(key != PortalLink.LinkKey) return;
            subject.transform.position = transform.position;
        }


        private void OnMovesIncresedEvent(int moves)
        {
            
        }

        public void ReverseTeleport(ITeleportable subject){
            OnReverseTeleport?.Invoke(PortalLink.LinkKey, subject.GetGameObject());
        }

        public void Teleport(ITeleportable subject){
        }

        public bool IsSender() => false;
    }
}