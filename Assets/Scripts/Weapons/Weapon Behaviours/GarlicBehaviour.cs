using System.Collections.Generic;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> markedEnemies;
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && !markedEnemies.Contains(collider.gameObject)) 
        {
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage());

            markedEnemies.Add(collider.gameObject);
        }
        else if (collider.CompareTag("Prop"))
        {
            if (collider.gameObject.TryGetComponent(out BreakableProps breakable) && !markedEnemies.Contains(collider.gameObject))
            {
                breakable.TakeDamage(GetCurrentDamage());
                markedEnemies.Add(collider.gameObject);
            }
        }
    }
    
}
