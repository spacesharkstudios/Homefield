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


    void Awake()
    {
        // set the graivty scale variable to the rb2d.gravityScale
        rb2d.gravityScale = gravityScale;
        damagePercent = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //animator.SetBool("isAttacking", true);

        //movement = new Vector2(Input.GetAxis("Horizontal") * movesSpeed, rb2d.velocity.y);
        // check the players state
        // check if the player is grounded

        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
            }
        }

        bool jump;
        jump = Input.GetKeyDown(KeyCode.Space);

        Move(grounded, jump);
    }

    private void Move(bool grounded, bool jump)
    {
        if (grounded)
        {
            if (jump)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x / 2, 0);
                rb2d.AddForce(new Vector2(0, jumpForce));
            }
            else
            {
            
            }
        }

        else if (!grounded)
        {
            rb2d.AddForce(new Vector2(0, -5));
        }
    }
}
