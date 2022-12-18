using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    //components
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    AudioSourceManager asm;

    //movement stuff
    public float speed;
    public float jumpForce;

    //groundcheck stuff
    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask isGroundLayer;
    public float groundCheckRadius;

    //variables
    Coroutine jumpForceChange;

    //Audio Clips
    public AudioClip Jump;
    public AudioClip Spin;
    public AudioClip Squish;

    public void StartJumpForceChange()
    {
        if (jumpForceChange == null)
        {
            jumpForceChange = StartCoroutine(JumpForceChange());
        }
        else
        {
            StopCoroutine(jumpForceChange);
            jumpForceChange = null;
            jumpForce /= 2;
            jumpForceChange = StartCoroutine(JumpForceChange());
        }
    }

    IEnumerator JumpForceChange()
    {
        jumpForce *= 2;

        yield return new WaitForSeconds(5.0f);

        jumpForce /= 2;
        jumpForceChange = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        asm = GetComponent<AudioSourceManager>();

        if (speed <= 0)
        {
            speed = 6.0f;
            Debug.Log("Speed Not Set - Default to 6");
        }

        if (jumpForce <= 0)
        {
            jumpForce = 300;
            Debug.Log("Jump Force Not Set - Default to 300");
        }

        if (!groundCheck)
        {
            groundCheck = GameObject.FindGameObjectWithTag("GroundCheck").transform;
            Debug.Log("Ground Check Not Set - Finding it Manually");
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.2f;
            Debug.Log("Ground Check Radius Not Set - Default to 0.2");
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0.0f) return;

        AnimatorClipInfo[] curPlayingClip = anim.GetCurrentAnimatorClipInfo(0);
        float hInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if (curPlayingClip.Length > 0)
        {
            if (Input.GetButtonDown("Fire1") && curPlayingClip[0].clip.name != "Fire")
                anim.SetTrigger("Fire");
            else if (curPlayingClip[0].clip.name == "Fire")
                rb.velocity = Vector2.zero;
            else
            {
                Vector2 moveDirection = new Vector2(hInput * speed, rb.velocity.y);
                rb.velocity = moveDirection;
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);

            asm.PlayOneShot(Jump, false);
        }

        if (!isGrounded && Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("JumpAttack");
            asm.PlayOneShot(Spin, false);
        }
        

        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);
        
        if (hInput != 0)
            sr.flipX = (hInput < 0);

        if (isGrounded)
            rb.gravityScale = 1;
    }

    public void IncreaseGravity()
    {
        rb.gravityScale = 5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Squish"))
        {
            collision.gameObject.GetComponentInParent<EnemyWalker>().Squish();

            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);

            if (Squish)
                asm.PlayOneShot(Squish, false);
        }
    }
}