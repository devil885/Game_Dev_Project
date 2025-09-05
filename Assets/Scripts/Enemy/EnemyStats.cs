using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    float currentHealth;
    float currentSpeed;
    float currentDamage;

    void Awake() 
    {
        currentHealth = enemyData.MaxHealth;
        currentSpeed = enemyData.MoveSpeed;
        currentDamage = enemyData.Damage;
    }

    public void TakeDamage(float dmg) 
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) 
        {
            Kill();
        }
    }

    public void Kill() 
    {
        Destroy(gameObject);
    }
}
