using UnityEngine;

public class KnifeBehaviour : ProjectileWeaponBehaviour
{
    KnifeController controller;

    protected override void Start()
    {
        base.Start();
        controller = FindFirstObjectByType<KnifeController>();
    }

    void Update()
    {
        transform.position += direction * controller.projectileSpeed * Time.deltaTime;
    }
}
