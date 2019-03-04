using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController_V4 : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public Animator animator;
    public bool grounded;
    public bool hit;
    public bool launched;
    public bool attacking;
    public bool jumping;
    public int numJumps = 0;
    public int hitstunFrames = 0;
    public float movesSpeed = 300f;
    public float jumpForce = 200f;
    public float maxAirSpeed = 5f;
    public float gravityScale;
    public float damagePercent;
    public Transform groundCheck;
    public static LayerMask whatIsGround;
    public static float groundedRadius = .1f;
    public Vector2 movement;


    void Start()
    {
        // set the graivty scale variable to the rb2d.gravityScale
        rb2d.gravityScale = gravityScale;
        damagePercent = 0f;
        
        hitstunFrames = 0;
        launched = false;
    }

    // Update is called once per frame
    void Update()
    {
        //movement = new Vector2(Input.GetAxis("Horizontal") * movesSpeed, rb2d.velocity.y);
        // check the players state
        // check if the player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
            }
        }

        // if the player is hit
        if (hitstunFrames > 0 && 1 ==2)
        {
            // set players velocity to 0
            rb2d.velocity = new Vector2(0, 0);

            // tell the animator that the player is being hit
            animator.SetBool("isHit", true);

            // reduce hitstun frames by 1
            hitstunFrames--;

            hit = true;
        }

        else if(hitstunFrames <= 0)
        {
            hit = false;

            // set movement vector to the player input * movement speed
            Move();
        }
 

    }

    public void Move()
    {
        // if grounded and not attacking and not jumping
        if (grounded && !attacking && !jumping)
        {
            // set x velocity to movement vecotor
            //movement = new Vector2(Input.GetAxis("Horizontal"), rb2d.velocity.y);
            rb2d.velocity = new Vector2(Input.GetAxis("Horizontal"), rb2d.velocity.y);

            // if the players x velocity is greater than or less than 0
            if (Mathf.Abs(rb2d.velocity.x) > 0)
            {
                // tell the animator that the player is running
                animator.SetBool("isRunning", true);
            }
            // else if the players x velocity is 0
            else
            {
                // tell the animator that the player is idle
                animator.SetBool("isRunning", false);
            }
        }

        // else if grounded and attacking and not jumping
        else if (grounded && attacking && !launched && !jumping)
        {
            // if the player has no directional inputs
                // neutral ground attack
                // tell the animator
            // else if the player has horizontal directional inputs
                // side light attack
                // tell the animator
            // else if the player is pressing down
                // down light attack
                // tell the animator
            // else if the player is pressing up
                // up light attack
                // tell the animator
        }

        // else if (not grounded or has jumps left) and not attacking and jumping
        else if ((!grounded || numJumps > 0) && !attacking && jumping)
        {
            // set the velocity to half the x velocity in the x and the jump force in the y
            rb2d.velocity = new Vector2(rb2d.velocity.x / 2, jumpForce);

            // tell the animator
            animator.SetBool("isJumping", true);
        }
        
        // else if not grounded and attacking and not jumping
        else if (!grounded && attacking && !jumping)
        {
            // if the player has no directional inputs
            // NAir
            // tell the animator
            // else if the player has horizontal directional inputs
            // SAir
            // tell the animator
            // else if the player is pressing down
            // DAir
            // tell the animator
            // else if the player is presisng up
            // Up air
            // tell the animator
        }
        
        // else if not grounded and not attacking and not jumping
        else if (!grounded && !attacking && !jumping)
        {
            // if the player is falling
            if (rb2d.velocity.y <= 0)
            {
                // set the gravity scale higher
                rb2d.gravityScale = gravityScale * 2.5f;
            }

            // else
            else
            {
                // set the gravity scale to normal
                rb2d.gravityScale = gravityScale;
            }

            // if the player is moveing at the maximum air x velocity velocity
            if (Mathf.Abs(rb2d.velocity.x) > maxAirSpeed)
            {
                // set the velocity to the max air speed and the y velocity
                rb2d.velocity = new Vector2(maxAirSpeed * (Mathf.Abs(rb2d.velocity.x)/rb2d.velocity.x), rb2d.velocity.y);
            }
            // else if the player is moving less than the maximum air x velocity
            else if (Mathf.Abs(rb2d.velocity.x) <= maxAirSpeed)
            {
                // add a force to the rigidbody2D
                rb2d.AddForce(new Vector2(Input.GetAxis("Horizontal") * (movesSpeed * .1f), 0));
            }

            // tell the animator that the player is in the air idle
        }
         
        // 
    }
}
