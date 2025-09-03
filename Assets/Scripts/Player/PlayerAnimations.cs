using UnityEngine;

public class Animations : MonoBehaviour
{
    Animator anim;
    PlayerMovement movement;
    SpriteRenderer spriteRender;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (movement.moveDirection.x != 0 || movement.moveDirection.y != 0)
        {
            anim.SetBool("isMoving", true);
            CheckSpriteDirection();
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    void CheckSpriteDirection() 
    {
        if (movement.lastMoveDirection.x < 0)
        {
            spriteRender.flipX = true;
        }
        else
        {
            spriteRender.flipX = false;
        }
    }
}
