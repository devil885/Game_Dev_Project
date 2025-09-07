using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    PlayerStats player;
    public InputAction playerControls;
    [HideInInspector]
    public Vector2 moveDirection;
    [HideInInspector]
    public Vector2 lastMoveDirection;
    [HideInInspector]
    public Vector2 lastMovedVector;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerStats>();
        lastMovedVector = new Vector2(1, 0f);
    }

    void Update()
    {
        InputManager();
    }

    void FixedUpdate()
    {
        Move();
    }

    void InputManager() 
    {
        if (GameManager.instance.isGameOver) 
        {
            return;
        }

       moveDirection = playerControls.ReadValue<Vector2>();

        if (moveDirection.x != 0)
        { 
            lastMoveDirection.x = moveDirection.x;
            lastMovedVector = new Vector2(lastMoveDirection.x, 0f);
        }

        if (moveDirection.y != 0) 
        {
            lastMoveDirection.y = moveDirection.y;
            lastMovedVector = new Vector2(0f, lastMoveDirection.y);
        }

        if (moveDirection.x != 0 && moveDirection.y != 0) 
        {
            lastMovedVector = new Vector2(moveDirection.x, moveDirection.y);
        }
    }

    void Move() 
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }
        body.linearVelocity =
         new Vector2(moveDirection.x * player.CurrentMoveSpeed, moveDirection.y * player.CurrentMoveSpeed);

    }
}
