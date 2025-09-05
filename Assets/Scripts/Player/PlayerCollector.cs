using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.TryGetComponent(out CollectableInterface collectable)) 
        {
            collectable.Collect();
        }
    }
}
