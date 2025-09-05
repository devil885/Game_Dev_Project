using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSeconds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
}
