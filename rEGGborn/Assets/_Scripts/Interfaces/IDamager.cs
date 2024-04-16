using Player;

namespace Interfaces{
    public interface IDamager : IInteractable{
        void Damage(PlayerController player);
        bool IsDamaging();
    }
}