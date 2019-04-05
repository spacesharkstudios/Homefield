using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class playerController_V3_P2 : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public Animator animator;
    public bool facingRight;
    public bool grounded;
    const float groundedRadius = 0.1f;
    public float gravityScale;
    public float move;
    [SerializeField] public float maxAirSpeed = 100f;
    [SerializeField] public float moveSpeed = 400f;
    [SerializeField] public float jumpForce = 300f;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform ceilingCheck;

    public UnityEvent onLandEvent;

    // Start is called before the first frame update
    void Awake()
    {
        gravityScale = rb2d.gravityScale;

        if (onLandEvent == null)
            onLandEvent = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                {
                    onLandEvent.Invoke();
                    rb2d.velocity = new Vector2(0, 0);
                }

            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            move = 1.0f;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            move = -1.0f;
        }
        else
        {
            move = 0.0f;
        }
        

        Move(move, 0, grounded, Input.GetKey(KeyCode.P), false, Input.GetKey(KeyCode.UpArrow));
    }

    private void Move(float move, int hitStunFrames, bool grounded, bool attacking, bool launched, bool jump)
    {
        bool hitStun;
        if (hitStunFrames > 0)
            hitStun = true;
        else
            hitStun = false;

        if (grounded && !launched && !attacking && !hitStun)
        {
            rb2d.velocity = new Vector2(move * moveSpeed * Time.fixedDeltaTime, 0);
            animator.SetBool("isJumping", false);

            if (Mathf.Abs(move) > 0)
                animator.SetBool("isRunning", true);
            else
                animator.SetBool("isRunning", false);
        }

        // if the player is not grounded, launched, or attacking
        if (!grounded && !launched && !attacking && !hitStun)
        {
            if (rb2d.velocity.x < Mathf.Abs(maxAirSpeed))
            {
                rb2d.AddForce(new Vector2(move * (moveSpeed * 2f) * Time.fixedDeltaTime, 0));
            }
            if (rb2d.velocity.x >= Mathf.Abs(maxAirSpeed))
            {
                rb2d.velocity = new Vector2(maxAirSpeed - .01f * (rb2d.velocity.x / (Mathf.Abs(rb2d.velocity.x))), rb2d.velocity.y);
            }

            if (rb2d.velocity.y <= 0)
                rb2d.gravityScale = gravityScale * 2f;
            if (rb2d.velocity.y > 0)
                rb2d.gravityScale = gravityScale;

            animator.SetBool("isJumping", true);
        }

        // if the player presses the jump key and is not launched or attacking
        if (grounded && jump && !launched && !attacking)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x / 2, 0);
            rb2d.AddForce(new Vector2(0, jumpForce));

        }

        // if the player presses the attack key on the ground

        if (attacking && !launched)
        {
            if (grounded)
            {
                if (move == 0 && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
                {
                    // neutral attack ground
                    // instantiate the hitbox prefab
                    // tell the animator what the player is doing
                }
                if (Mathf.Abs(move) > 0 && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
                {
                    // side attack ground
                    // instantiate the hitbox prefab
                    // tell the animator what the player is doing
                }
                if (Input.GetKey(KeyCode.S))
                {
                    // down attack ground
                    // instantiate the hitbox prefab
                    // tell the animator what the player is doing
                }
                if (Input.GetKey(KeyCode.W))
                {
                    // up attack ground
                    // instantiate the hitbox prefab
                    // tell the animator what the player is doing
                }
            }
            if (!grounded)
            {
                if (move == 0 && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
                {
                    // neutral attack in air
                    // instantiate the hitbox prefab
                    // tell the animator what the player is doing
                }
                if (Mathf.Abs(move) > 0 && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
                {
                    // side attack in air
                    // instantiate the hitbox prefab
                    // tell the animator what the player is doing
                }
                if (Input.GetKey(KeyCode.S))
                {
                    // down attack in air
                    // instantiate the hitbox prefab
                    // tell the animator what the player is doing
                }
                if (Input.GetKey(KeyCode.W))
                {
                    // up attack in air
                    // instantiate the hitbox prefab
                    // tell the animator what the player is doing
                }
            }
        }

        if (launched)
        {
            // play launched animation
            // set the physics material to a bouncey material
            // unlock rotation
        }
        if (!launched)
        {
            // set to vertical
            // lock rotation
        }

        // if the player is not launched, flip the player based on their input
        if (facingRight && move > 0 && !launched)
            Flip();
        if (!facingRight && move < 0 && !launched)
            Flip();
    }

    // method to flip the player around 180 degrees when called
    private void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}

// You're still here?

// Code's over.

// Go home.

// Go