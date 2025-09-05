using UnityEngine;

public class HealthPotion : PickUp, CollectableInterface
{
    public int healthToRestore;
    public void Collect() 
    {
        PlayerStats player = FindFirstObjectByType<PlayerStats>();
        player.RestoreHealth(healthToRestore);
    }
}
