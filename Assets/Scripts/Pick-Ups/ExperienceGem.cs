using UnityEngine;

public class ExperienceGem : PickUp, CollectableInterface
{
    public int experienceAmount;

    public void Collect() 
    {
        PlayerStats player = FindFirstObjectByType<PlayerStats>();
        player.IncreaseExperience(experienceAmount);
    }
}
