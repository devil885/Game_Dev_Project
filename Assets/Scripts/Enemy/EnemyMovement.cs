using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;

    Vector2 knockbackVelocity;
    float knockbackDuration;

    void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>().transform;
        enemy = GetComponent<EnemyStats>();
    }

    void Update()
    {
        if (knockbackDuration > 0) 
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
        }
    }

    public void Knockback(Vector2 velocity,float duration) 
    {
        if (knockbackDuration > 0) return;
        
        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
