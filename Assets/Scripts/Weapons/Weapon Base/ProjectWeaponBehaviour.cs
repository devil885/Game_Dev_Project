using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    protected Vector3 direction;
    public float destroyAfterSeconds;

    protected float currentDamage;
    protected float currentProjectileSpeed;
    protected float currentCooldown;
    protected float currentPierce;

    void Awake() 
    {
        currentDamage = weaponData.Damage;
        currentProjectileSpeed = weaponData.ProjectileSpeed;
        currentPierce = weaponData.Pierce;
        currentCooldown = weaponData.BaseCooldown;
    }
    
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);    
    }

    public void DirectionChecker(Vector3 dir) 
    {
        direction = dir;
        float directionX = direction.x;
        float directionY = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if (directionX < 0 && directionY == 0) //left
        {
            scale.x *= -1;
            scale.y *= -1;
        }
        else if(directionX == 0 && directionY < 0) //down
        {
            scale.y *= -1;
        }
        else if (directionX == 0 && directionY > 0) //up
        {
            scale.x *= -1;
        }
        else if (directionX > 0 && directionY > 0) //up right
        {
            rotation.z = 0f;
        }
        else if (directionX < 0 && directionY > 0) //up left
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = -90f;
        }
        else if (directionX > 0 && directionY < 0) //down right
        {
            rotation.z = -90f;
        }
        else if (directionX < 0 && directionY < 0) //down left
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = 0f;
        }
       
        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.CompareTag("Enemy")) 
        {
            EnemyStats enemy = collider.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);
            ReducePierce();
        }
    }

    void ReducePierce() 
    {
        currentPierce--;
        if (currentPierce <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
