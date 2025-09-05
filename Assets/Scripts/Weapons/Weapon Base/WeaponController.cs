using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon stats")]
    public GameObject prefab;
    public float damage;
    public float projectileSpeed;
    public float pierce;
    public float baseCooldown;
    float currentCooldown;
    protected PlayerMovement playerMovement;
    
   protected virtual void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        currentCooldown = baseCooldown;
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
        currentCooldown = baseCooldown;
    } 
}
