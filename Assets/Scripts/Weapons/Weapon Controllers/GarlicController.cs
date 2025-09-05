using UnityEngine;

public class GarlicController : WeaponController
{
    
   protected override void Start()
   {
        base.Start();
   }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedGralic = Instantiate(weaponData.Prefab);
        spawnedGralic.transform.position = transform.position;
        spawnedGralic.transform.parent = transform;
    }
}
