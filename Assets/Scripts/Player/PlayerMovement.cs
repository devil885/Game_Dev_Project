using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D body;
    public InputAction playerControls;
    [HideInInspector]
    public Vector2 moveDirection;
    [HideInInspector]
    public Vector2 lastMoveDirection;

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
    }


    // Update is called once per frame
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
       moveDirection = playerControls.ReadValue<Vector2>();

        if (moveDirection.x != 0)
        { 
            lastMoveDirection.x = moveDirection.x;
        }

        if (moveDirection.y != 0) 
        {
            lastMoveDirection.y = moveDirection.y;
        }
    }

    void Move() 
    {
        body.linearVelocity =
         new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

    }
}
