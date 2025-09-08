using UnityEngine;

public class HealthPotion : PickUp
{
    public int healthToRestore;

    public override void Collect() 
    {
        if (!hasBeenCollected)
        {
            PlayerStats player = FindFirstObjectByType<PlayerStats>();
            player.RestoreHealth(healthToRestore);
            base.Collect();
        }
    }
}
