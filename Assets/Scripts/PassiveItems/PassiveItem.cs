using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats player;
    public PassiveItemScriptableObject passiveItemData;

    void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();
        ApplyModifier();
    }

    protected virtual void ApplyModifier() 
    {
    
    }
}
