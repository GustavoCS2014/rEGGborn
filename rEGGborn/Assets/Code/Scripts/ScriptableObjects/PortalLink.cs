using UnityEngine;

namespace Reggborn.Settings
{
    [CreateAssetMenu(fileName = "PortalLink", menuName = "Object Settings/ New Portal Link", order = 0)]
    public class PortalLink : ScriptableObject
    {
        [Tooltip("The key used to link two portals")]
        [SerializeField] private uint linkKey;
        public uint LinkKey => linkKey;
    }
}