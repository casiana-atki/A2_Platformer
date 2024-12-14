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
    //J7|T3 Variables
    public float coyoteTime = 0.2f; 
    private float coyoteTimeCounter = 0f;
    //A2|T1 Variable
    private bool isOnDirt = false;
    //A2|T2 Variables
    public float dashMultiplier = 2f; 
    public float dashDuration = 0.3f; 
    public float dashCooldown = 5f; 
    private bool canDash = true; 

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

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; 
        }

        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f)
        {
            Jump();
        }

        //A2|T2 - Dash 
        if (Input.GetKeyDown(KeyCode.E) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    //A2|T2 Dash Coroutine 
    private IEnumerator Dash()
    {
        canDash = false; //Make it so the player is currently dashing & cannot dash again until the cooldown is over. 

        float originalMoveSpeed = moveSpeed; 
        moveSpeed *= dashMultiplier; //Multiply the movespeed by the multipler so the player is going 3 times as fast. 

        yield return new WaitForSeconds(dashDuration); //Really low value since it should be a quick dash. 

        moveSpeed = originalMoveSpeed; //Return the movespeed to the original movespeed 

        yield return new WaitForSeconds(dashCooldown);  //Player must wait 5 seconds before they can dash again. 
        canDash = true; 
    }

    private void MovementUpdate(Vector2 playerInput)
    {//J6|T1 - Basic Movement & J7|T2

        float currentMoveSpeed = isOnDirt ? moveSpeed / 2f : moveSpeed; //For A2|T1 Movement speed is halved depending on whether or not the player is on the dirt. When true, player speed is halved so they walk slower.
        rb.velocity = new Vector2(playerInput.x * currentMoveSpeed, rb.velocity.y);

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

        //Commented this out to save my console from being spammed. Feel free to uncomment it to check the velocity. 
        //Debug.Log("Velocity: " + rb.velocity);

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
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Dirt"))
        {
            isGrounded = true;

            if (other.gameObject.CompareTag("Dirt"))
            {
                isOnDirt = true; //Player is touching the dirt
                //Debug.Log("Dirt is touched. Watch out for worms!");
            }

            else
            {
                isOnDirt = false; //Player is not touching the dirt. 
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Dirt"))
        {
            isGrounded = false; 
        }
        if (other.gameObject.CompareTag("Dirt"))
        {
            isOnDirt = false; //Player has left the dirt & is no longer on it. 
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
