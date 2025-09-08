using UnityEngine;

public class PickUp : MonoBehaviour, CollectableInterface
{
    protected bool hasBeenCollected = false;

    public virtual void Collect() 
    {
        hasBeenCollected = true;
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
