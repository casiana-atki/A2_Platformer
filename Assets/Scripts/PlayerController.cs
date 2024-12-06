using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        left, right
    }

    //Journal 6 - Variables
    public float moveSpeed = 5f; // Public variable for movement speed
    public float jumpForce = 6f;
    private Rigidbody2D rb;
    private FacingDirection currentFacingDirection = FacingDirection.right;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded = false;

    //J7|T1 Variables
    public float apexHeight = 0.65f; // Max jump height
    public float apexTime = 0.4f; // Time to reach the apex
    private float originalGravityScale; // Store the original gravity scale
    //J7|T2 Variables
    public float terminalSpeed = -10f;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalGravityScale = rb.gravityScale; 
    }

    void Update()
    {
        rb.freezeRotation = true;
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
    {//J6|T1 - Basic Movement & J7|T2
        rb.velocity = new Vector2(playerInput.x * moveSpeed, rb.velocity.y);

        if (rb.velocity.y < terminalSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, terminalSpeed);
        }

        if (playerInput.x > 0)
        {
            currentFacingDirection = FacingDirection.right;
        }
        else if (playerInput.x < 0)
        {
            currentFacingDirection = FacingDirection.left;
        }

        Debug.Log("Velocity: " + rb.velocity);

        //J6|T2 - Animation Updates
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = (currentFacingDirection == FacingDirection.left);
        }
    }

    //J7:T1 - Dynamic Jumping
    private void Jump()
    {
        float initialJumpVelocity = Mathf.Sqrt(2 * apexHeight * originalGravityScale * 10f / rb.gravityScale);
        rb.velocity = new Vector2(rb.velocity.x, initialJumpVelocity);
        rb.gravityScale = originalGravityScale * 0.5f;
        StartCoroutine(RestoreGravity());
    }

    //J7:T1 - Dynamic Jumping
    private IEnumerator RestoreGravity()
    {
        yield return new WaitForSeconds(apexTime);
        rb.gravityScale = originalGravityScale; 
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

    //J6|T2 - Animation Updates
    public bool IsWalking()
    {
        return Mathf.Abs(rb.velocity.x) > 0.1f;
    }
    
    //Updated for J6|T3 - Groundedness
    public bool IsGrounded()
    {
        return isGrounded;
    }

    //J6|T2 - Animation Updates 
    public FacingDirection GetFacingDirection()
    {
        return currentFacingDirection;
    }
}
