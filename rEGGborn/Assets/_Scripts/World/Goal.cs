using System;
using Objects;
using Player;
public class Goal : GridObject
{
    public static event Action OnGoalEnter;
    public override uint CollisionPriority { get; protected set; } = 0;
    public override CollisionType Type { get; protected set; } = CollisionType.Goal;
    public override bool GhostInteractable { get; protected set; } = false;

    public override void Interact(PlayerController player){
        OnGoalEnter?.Invoke();
    }
}