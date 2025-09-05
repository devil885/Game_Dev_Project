using UnityEngine;

public class HealthPotion : MonoBehaviour, CollectableInterface
{
    public int healthToRestore;
    public void Collect() 
    {
        PlayerStats player = FindFirstObjectByType<PlayerStats>();
        player.RestoreHealth(healthToRestore);
        Destroy(gameObject);
    }
}
