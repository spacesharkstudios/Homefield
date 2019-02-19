using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D rb2d;              // Create a Rigidbody2D reference variable called rb2d
    private SpriteRenderer spriteRenderer; // Create a SpriteRenderer reference variable called spriteRenderer
    private Animator animator;              // Create an Animator reference variable called animator
    public float moveSpeed = 10f;          // Create a public float called moveSpeed
    private bool facingRight = true;       // Create a boolean value called facingRight and set it to true
    public float jumpForce = 300f;         // Create a float value for jump force
    private bool isLaunched;
    public float movementSpeed;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();              // Get the players Rigidbody2D and store it in rb2d
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the players SpriteRenderer and store it in spriteRenderer
        animator = GetComponent<Animator>();
        moveSpeed = 300f;
        isLaunched = false;
    }

    private void FixedUpdate()
    {
        // Get keyboard input
        float horizontalMovement = Input.GetAxis("Horizontal"); // "A" returns a -1 and "D" returns a 1

        // Create a Vector2 object to add force to the player
        Vector2 movement = new Vector2(horizontalMovement * moveSpeed * Time.fixedDeltaTime, rb2d.velocity.y);
        // Time.fixedDeltaTime is used to make the game run similarly on all systems reguardless of framerate

        // Add the new Vector2 to the players velocity
        rb2d.velocity = movement;

        // Check to see what direction the player is facing
        if (horizontalMovement < 0) // Check if the player is moving left
        {
            facingRight = false;
            spriteRenderer.flipX = true;
        }
        if (horizontalMovement > 0) // Check if the player is moving right
        {
            facingRight = true;
            spriteRenderer.flipX = false;
        }

        if (Input.GetKey(KeyCode.Space) && rb2d.velocity.y == 0 && !isLaunched)
        {
            Vector2 jump = new Vector2(rb2d.velocity.x, jumpForce);
            rb2d.AddForce(jump);
            Debug.Log("You hit the jump button, asshole");
        }

        if (rb2d.velocity.x > 0 || rb2d.velocity.x < 0)
        {
            animator.SetBool("isRunning", true);
        }
        else if (rb2d.velocity.x == 0)
        {
            animator.SetBool("isRunning", false);
        }
        
    }

    public void Launch(Rigidbody2D rb2d2)
    {
        isLaunched = true;
        Vector2 launch = new Vector2(100, 300);
        rb2d2.AddForce(launch);

    }
}
