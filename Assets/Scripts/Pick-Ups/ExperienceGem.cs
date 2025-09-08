using UnityEngine;

public class ExperienceGem : PickUp
{
    public int experienceAmount;

    public override void Collect() 
    {
        if (!hasBeenCollected)
        {
            PlayerStats player = FindFirstObjectByType<PlayerStats>();
            player.IncreaseExperience(experienceAmount);
            base.Collect();
        }
    }
}
