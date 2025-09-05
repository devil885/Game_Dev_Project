using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;

    protected PlayerMovement playerMovement;
    
   protected virtual void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        currentCooldown = weaponData.BaseCooldown;
    }

    
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f) 
        {
            Attack();
        }
    }

    protected virtual void Attack() 
    {
        currentCooldown = weaponData.BaseCooldown;
    } 
}
