using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        left, right
    }

    //Journal 7 - Task 1 Variables
    public float moveSpeed = 5f; // Public variable for movement speed
    private Rigidbody2D rb;      // Reference to Rigidbody2D
    private FacingDirection currentFacingDirection = FacingDirection.right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), 0f);
        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {//J7|T1 - Basic Movement
        rb.velocity = new Vector2(playerInput.x * moveSpeed, rb.velocity.y);
        if (playerInput.x > 0)
        {
            currentFacingDirection = FacingDirection.right;
        }
        else if (playerInput.x < 0)
        {
            currentFacingDirection = FacingDirection.left;
        }

        //J7|T2 - Animation Updates
        if (currentFacingDirection == FacingDirection.right)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    //J7|T2 - Animation Updates
    public bool IsWalking()
    {
        return Mathf.Abs(rb.velocity.x) > 0.1f;
    }
    public bool IsGrounded()
    {
        return false;
    }

    //J7|T2 - Animation Updates 
    public FacingDirection GetFacingDirection()
    {
        return currentFacingDirection;
    }
}
