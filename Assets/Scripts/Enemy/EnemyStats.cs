using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    public float currentHealth;
    public float currentMoveSpeed;
    public float currentDamage;
    public float despawnDistance = 20f;
    Transform player;

    void Awake() 
    {
        currentHealth = enemyData.MaxHealth;
        currentMoveSpeed = enemyData.MoveSpeed;
        currentDamage = enemyData.Damage;
    }

    void Start() 
    {
        player = FindFirstObjectByType<PlayerStats>().transform;
    }

    void Update() 
    {
        if (Vector2.Distance(transform.position, player.position) >= despawnDistance) 
        {
            ReturnEnemy();
        }
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
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        spawner.OnEnemyKilled();

        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);

        }
    }

    void ReturnEnemy() 
    {
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        transform.position = player.position + spawner.relativeSpawnPoints[UnityEngine.Random.Range(0, spawner.relativeSpawnPoints.Count)].position;
    }
   
}
