using Objects.Settings;

namespace Interfaces{
    public interface IPortal{
        PortalLink PortalLink{
            get;
            set;
        }

        void Teleport(ITeleportable subject);
        void ReverseTeleport(ITeleportable subject);
        bool IsSender();
    }
}