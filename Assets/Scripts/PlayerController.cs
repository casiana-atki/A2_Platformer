using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        left, right
    }

    //Journal 7 - Variables
    public float moveSpeed = 5f; // Public variable for movement speed
    public float jumpForce = 6f;
    private Rigidbody2D rb;      // Reference to Rigidbody2D
    private FacingDirection currentFacingDirection = FacingDirection.right;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), 0f);
        MovementUpdate(playerInput);

        if (Input.GetButtonDown("Jump") && IsGrounded())  
        {
            Jump();
        }
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
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = (currentFacingDirection == FacingDirection.left);
        }
    }

    //J7:T3 - Groundedness
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) 
        {
            isGrounded = true; 
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) 
        {
            isGrounded = false; 
        }
    }

    //J7|T2 - Animation Updates
    public bool IsWalking()
    {
        return Mathf.Abs(rb.velocity.x) > 0.1f;
    }
    
    //Updated for J7|T3 - Groundedness
    public bool IsGrounded()
    {
        return isGrounded;
    }

    //J7|T2 - Animation Updates 
    public FacingDirection GetFacingDirection()
    {
        return currentFacingDirection;
    }
}
