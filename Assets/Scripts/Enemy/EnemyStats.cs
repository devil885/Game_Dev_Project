using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    public float currentHealth;
    public float currentMoveSpeed;
    public float currentDamage;
    public float despawnDistance = 20f;
    Transform player;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1,0,0,1);
    public float damageFlashDuration = 0.2f;
    public float deathFadeTime = 0.6f;
    Color originalColor;
    SpriteRenderer spriteRenderer;
    EnemyMovement movement;

    void Awake() 
    {
        currentHealth = enemyData.MaxHealth;
        currentMoveSpeed = enemyData.MoveSpeed;
        currentDamage = enemyData.Damage;
    }

    void Start() 
    {
        player = FindFirstObjectByType<PlayerStats>().transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        movement = GetComponent<EnemyMovement>();
    }

    void Update() 
    {
        if (Vector2.Distance(transform.position, player.position) >= despawnDistance) 
        {
            ReturnEnemy();
        }
    }

    public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f) 
    {
        currentHealth -= dmg;
        StartCoroutine(DamageFlash());

        if (knockbackForce > 0) 
        {
            Vector2 direction = (Vector2)transform.position - sourcePosition;
            movement.Knockback(direction.normalized * knockbackForce, knockbackDuration);
        }

        if (currentHealth <= 0) 
        {
            Kill();
        }
    }

    IEnumerator DamageFlash() 
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
    }

    IEnumerator KillFade() 
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float time = 0, originalAlpha = spriteRenderer.color.a;


        while (time < deathFadeTime) 
        {
            yield return wait;
            time += Time.deltaTime;

            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, (1 - time / deathFadeTime) * originalAlpha);
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
