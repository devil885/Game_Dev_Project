using UnityEngine;

public class MeleeWeaponBehaviour : MonoBehaviour
{
    public float destroyAfterSeconds;
    public WeaponScriptableObject weaponData;

    protected float currentDamage;
    protected float currentProjectileSpeed;
    protected float currentCooldown;
    protected float currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentProjectileSpeed = weaponData.ProjectileSpeed;
        currentPierce = weaponData.Pierce;
        currentCooldown = weaponData.BaseCooldown;
    }

    protected virtual void Start()
    {
        Destroy(gameObject,destroyAfterSeconds);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.CompareTag("Enemy")) 
        {
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
        }
    }

    
}
