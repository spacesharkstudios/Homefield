using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playerController_V2 : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isFacingRight;
    public bool grounded;
    const float groundedRadius = 0.2f;
    public float gravityScale;
    public float maxAirSpeed = 100f;
    public float maxGroundSpeed;
    [SerializeField] public float movementSpeed = 400f;
    [SerializeField] public float jumpForce = 150f;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public Transform ceilingCheck;

    public UnityEvent onLandEvent;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gravityScale = rb2d.gravityScale;

        if (onLandEvent == null)
            onLandEvent = new UnityEvent();
    }

    // Update is called once per physics calculation
    private void FixedUpdate()
    {
        bool wasGouunded = grounded;
        grounded = false;

        // The player is "grounded" once the a circle cast overlaps anything dubbed as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGouunded)
                    onLandEvent.Invoke();
            }
        }

        Move(Input.GetAxis("Horizontal"), .75f, false, Input.GetKey(KeyCode.Space));
    }


    // method to move the player
    // parameters: move, air move, launched, jump
    // move is a value between -1 and 1, deturmines the direction the player will move
    // airMove is the amound the acceleration ratio of in air acceleration over ground acceleration
    private void Move(float move, float airMove, bool launched, bool jump)
    {
        // check if the player is on the ground and not launched
        if (grounded && !launched)
        {
            // tell the animator that the player is not jumping
            animator.SetBool("isJumping", false);

            // set the velocity to a new vector based on the directional key that the player is pressing
            rb2d.velocity = new Vector2(move * movementSpeed * Time.fixedDeltaTime, rb2d.velocity.y);

            // if the player has an x velocity greater than 0 then tell the animator that they are running
            if (Mathf.Abs(rb2d.velocity.x) > 0)
                animator.SetBool("isRunning", true);
            if (rb2d.velocity.x == 0)
                animator.SetBool("isRunning", false);

        }

        // check if the player is not grounded and not launched
        if (!grounded && !launched)
        {
            // check if the player is trying to move faster than the maximum air speed

            // if the player's x velocity while not grounded is less than or equal to the maximum air speed then add force
            if (Mathf.Abs(rb2d.velocity.x) <= maxAirSpeed)
            {
                rb2d.AddForce(new Vector2(move * (movementSpeed * 2f) * Time.fixedDeltaTime, 0));
            }
            // if the player is trying to move any faster than the maximum air speed then stop adding force
            else
            {
                rb2d.velocity = new Vector2(maxAirSpeed * (Mathf.Abs(rb2d.velocity.x) / rb2d.velocity.x), rb2d.velocity.y);
                // |rb2d.velocity.x|/rb2d.velocity.x is used to deturmine the direction of the vector instead of move because the value of move can be changed in mid air
            }


            // if the player is falling make the gravity scale higher
            if(rb2d.velocity.y < 0)
            {
                rb2d.gravityScale = gravityScale * 2f;
            }
            // if the player is rising keep the gravity scale normal
            if (rb2d.velocity.y >= 0)
            {
                rb2d.gravityScale = gravityScale;
            }

            // tell the animator that the player is in the air
            animator.SetBool("isJumping", true);
        }

        // let the player jump if they are grounded and they press the jump key
        if(jump && grounded)
        {
            rb2d.AddForce(new Vector2(0, jumpForce)); // add a vertical force with no horizontal force
        }

        // set isFacingRight based on if the player's x velocity is positive or negative
        if (rb2d.velocity.x < 0)
            isFacingRight = true;
        if (rb2d.velocity.x > 0)
            isFacingRight = false;
        
        // spriteRenderer.flipX takes a boolean value. Pass in the isFacingRight value
        spriteRenderer.flipX = isFacingRight;
    }
}
