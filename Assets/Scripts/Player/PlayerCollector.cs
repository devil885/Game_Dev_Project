using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;

    void Start() 
    {
        player = FindFirstObjectByType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    void Update() 
    {
        playerCollector.radius = player.CurrentPickUpRange;
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.TryGetComponent(out CollectableInterface collectable)) 
        {
            Rigidbody2D body = collider.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (transform.position - collider.transform.position).normalized;
            body.AddForce(forceDirection*pullSpeed);

            collectable.Collect();
        }
    }
}
