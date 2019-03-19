using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class playerController_V3 : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public Animator animator;
    public bool facingRight;
    public bool grounded;
    const float groundedRadius = 0.1f;
    public float gravityScale = 1;
    public float move;
    public bool canMove = true;
    public bool launched = false;
    public float health = 0.0f;
    public GameObject neutralLight;
    [SerializeField] public float maxAirSpeed = 100f;
    [SerializeField] public float moveSpeed = 400f;
    [SerializeField] public float jumpForce = 300f;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform cleetusCenter;

    public UnityEvent onLandEvent;

    // Start is called before the first frame update
    void Awake()
    {
        rb2d.gravityScale = 1;
        gravityScale = rb2d.gravityScale;
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

        move = Input.GetAxis("Horizontal");

        Move(move, 0, grounded, Input.GetKeyDown(KeyCode.P), launched, Input.GetKeyDown(KeyCode.Space));
    }

    private void Move(float move, int hitStunFrames, bool grounded, bool attacking, bool launched, bool jump)
    {
        bool hitStun;
        if (hitStunFrames > 0)
            hitStun = true;
        else
            hitStun = false;

        if (hitStun)
        {
            // set the hitsutn animation
            rb2d.velocity = new Vector2(0, 0);
            hitStunFrames--;
        }

        else if (grounded && !launched && !attacking && !hitStun && canMove)
        {
            rb2d.velocity = new Vector2(move * moveSpeed * Time.fixedDeltaTime, 0);
            animator.SetBool("isJumping", false);

            if (Mathf.Abs(move) > 0)
                animator.SetBool("isRunning", true);
            else
                animator.SetBool("isRunning", false);

            if (jump)
            {
                rb2d.AddForce(new Vector2(0, jumpForce));
            }
        }

        // if the player is not grounded, launched, or attacking
        else if (!grounded && !launched && !attacking && !hitStun && canMove)
        {
            if (Mathf.Abs(rb2d.velocity.x) < maxAirSpeed)
            {
                rb2d.AddForce(new Vector2(move * (moveSpeed * 2f) * Time.fixedDeltaTime, 0));
            }
            if (rb2d.velocity.x >= Mathf.Abs(maxAirSpeed))
            {
                rb2d.velocity = new Vector2(maxAirSpeed - .01f * (rb2d.velocity.x/(Mathf.Abs(rb2d.velocity.x))), rb2d.velocity.y);
            }

            if (rb2d.velocity.y <= 0)
                rb2d.gravityScale = gravityScale + 1 * 2f;
            if (rb2d.velocity.y > 0)
                rb2d.gravityScale = gravityScale + 1;

            animator.SetBool("isJumping", true);
        }

        // if the player presses the attack key on the ground

        else if (attacking && !launched)
        {
            float startTime = Time.time;
            if (grounded)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 0 && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
                {
                    
                    // neutral ground attack
                    StartCoroutine(NeutralGround());
                    
                }
                else if (Mathf.Abs(move) > 0 && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
                {

                    // side attack ground
                    rb2d.velocity = new Vector2(5 * Input.GetAxisRaw("Horizontal"), 0);

                    StartCoroutine(SideGround());
                    
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    // down light
                    StartCoroutine(DownGround());
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    // up attack ground
                    StartCoroutine(UpGround());
                }
            }
            else if (!grounded)
            {
                if (move == 0 && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
                {
                    // neutral attack in air
                    StartCoroutine(NeutralAir());
                }
                if (Mathf.Abs(move) > 0 && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
                {
                    // side attack in air
                    StartCoroutine(SideAir());
                }
                if (Input.GetKey(KeyCode.S))
                {
                    // down attack in air
                    StartCoroutine(DownAir());
                }
                if (Input.GetKey(KeyCode.W))
                {
                    // up attack in air
                    StartCoroutine(UpAir());
                }
            }
        }
        if (!attacking)
        {
            animator.SetBool("isAttacking", false);
            //animator.SetBool("isSideLight", false);
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

    public IEnumerator NeutralGround()
    {
        canMove = false;
        rb2d.velocity = new Vector2(0, 0);
        animator.SetBool("isAttacking", true);
        yield return new WaitForSecondsRealtime(.15f);
        Instantiate(neutralLight, cleetusCenter);
        yield return new WaitForSecondsRealtime(.15f);
        canMove = true;
    }

    public IEnumerator SideGround()
    {
        canMove = false;
        animator.SetBool("isSideLight", true);
        yield return new WaitForSecondsRealtime(.15f);
        //Instantiate(SideGround, cleetusCenter);
        yield return new WaitForSecondsRealtime(.15f);
        animator.SetBool("isSideLight", false);
        canMove = true;
    }

    public IEnumerator DownGround()
    {
        canMove = false;
        animator.SetBool("isDownGround", true);
        rb2d.velocity = new Vector2(0, 0);
        yield return new WaitForSecondsRealtime(1);
        //Instantiate(DownGround, cleetusCenter);
        yield return new WaitForSecondsRealtime(.15f);
        animator.SetBool("isDownGround", false);
        canMove = true;
    }

    public IEnumerator UpGround()
    {
        canMove = false;
        animator.SetBool("isUpGround", true);
        rb2d.velocity = new Vector2(0, 0);
        yield return new WaitForSecondsRealtime(1);
        //Instantiate(UpGround, cleetusCenter);
        yield return new WaitForSecondsRealtime(.15f);
        animator.SetBool("isUpGround", false);
        canMove = true;
    }

    // moving in air attacks is fine, just not during ground attacks
    public IEnumerator NeutralAir()
    {
        // player can move during in air attacking
        yield return new WaitForSecondsRealtime(1);
    }

    public IEnumerator SideAir()
    {
        // player can move during in air attacking
        yield return new WaitForSecondsRealtime(1);
    }

    public IEnumerator DownAir()
    {
        // player can move during in air attacking
        yield return new WaitForSecondsRealtime(1);
    }

    public IEnumerator UpAir()
    {
        // player can move during in air attacking
        yield return new WaitForSecondsRealtime(1);
    }
    
    // create WaitForSecondsRealTime to be thrown during coroutines
    private object WaitForSecondsRealtime(float time)
    {
        throw new NotImplementedException();
    }
}



// You're still here?

// Code's over.

// Go home.

// Go.

