using UnityEngine;

public class Chest : MonoBehaviour
{
    InventoryManager inventory;

    void Start()
    {
        inventory = FindFirstObjectByType<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Player")) 
        {
            OpenChest();
            Destroy(gameObject);
        }
    }

    public void OpenChest() 
    {
        if (inventory.GetPossibleEvolutions().Count <= 0) 
        {
            Debug.LogWarning("NO Available evolutions");
            return;
        }
        WeaponEvolutionBlueprint toEvolve = inventory.GetPossibleEvolutions()[UnityEngine.Random.Range(0, inventory.GetPossibleEvolutions().Count)];
        inventory.EvolveWeapon(toEvolve);
    }

    void Update()
    {
        
    }
}
