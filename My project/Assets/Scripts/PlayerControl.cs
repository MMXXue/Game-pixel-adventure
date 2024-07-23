using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;

    Animator anim;

    AudioSource audioS;
    public AudioClip running, jumpping, mushroom;

    public float PlayerSpeed = 5.0f;
    [Range(1, 10)]
    public float jumpSpeed = 5f;
    // player on ground/sky
    public bool isGrounded;
    // ground check point
    public Transform groundCheck;
    // mask using for ground check
    public LayerMask ground;
    // the gravity of fall time
    public float fallAddition = 3.5f;
    // the gravity of jump time
    public float jumpAddition = 1.5f;
    public ParticleSystem playerPS;
    // the count of jump
    private int jumpCount;


    private float moveX;
    // using for flip
    private bool facingRight = true;
    private bool moveJump;
    // hold-on jump time
    private bool jumpHold;

    private bool doubleJumpUnlocked = false;

    public GameObject JumpImage;



    bool isJump;
    [SerializeField] private Transform killPoint;

    private enum playerState { idle, run, jump, fall, doublejump }; // 0,1,2,3


    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint; // The position of bullets was fired


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        killPoint = transform.Find("killPoint");
        audioS = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        // player horizontal axis movement
        moveX = Input.GetAxis("Horizontal");//-1~1
        // jump input
        moveJump = Input.GetButtonDown("Jump");
        jumpHold = Input.GetButton("Jump");

        //Debug.Log("doubleJumpUnlocked: " + doubleJumpUnlocked);
        if (doubleJumpUnlocked)
        {
            JumpImage.SetActive(true);
        }
        else
        {
            JumpImage.SetActive(false);
        }

        if (moveJump && jumpCount > 0)
        {
            isJump = true;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            //Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Bullet>().SetDirection(transform.localScale.x > 0);
        }

    }

    private void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        Move();
        Jump();
        Enemy();
        playerAnim();
    }


    private void Move()
    {
        // add player's animator (run and stand status transfer)
        anim.SetFloat("speed", Mathf.Abs(moveX));

        // player movement
        rb.velocity = new Vector2(moveX * PlayerSpeed, rb.velocity.y);

        // facing left & move right
        if (facingRight == false && moveX > 0)
        {
            Flip();
        }
        // facing right & move left
        else if (facingRight == true && moveX < 0)
        {
            Flip();
        }
    }


    // Change player scale
    private void Flip()
    {
        // audio (footstep)
        audioS.clip = running;
        audioS.Play();

        PPS();
        facingRight = !facingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }




    // improve jump
    private void Jump()
    {
        if (isGrounded)
        {
            jumpCount = 2;
        }

        if (isJump)
        {

            if (doubleJumpUnlocked || isGrounded)
            {
                audioS.clip = jumpping;
                audioS.Play();
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                jumpCount--;
                isJump = false;
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallAddition;
        }
        //// when long time hold space(jump)
        //else if (rb.velocity.y > 0 && !jumpHold)
        //{
        //    rb.gravityScale = jumpAddition;
        //}
        else
        {
            rb.gravityScale = 1f;
        }
    }


    // dust particles
    void PPS()
    {
        //playerPS.Play();

        if (playerPS != null)
        {
            playerPS.Play();
        }
        else
        {
            Debug.LogError("playerPS is null");
        }
    }

    // for enemy
    private void Enemy()
    {
        Collider2D enemy = Physics2D.OverlapCircle(killPoint.position, 0.3f, LayerMask.GetMask("Enemy"));
        // If the character steps on it, destroy the mushroom
        if (enemy == null || enemy.tag == "FinalBoss")
        {
            return;
        }
        else
        {
            audioS.clip = mushroom;
            audioS.Play();

            Destroy(enemy.gameObject);
        }

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        // jump
        rb.AddForce(new Vector2(0, 300f));
    }


    // animator action
    void playerAnim()
    {
        playerState state;

        if (Mathf.Abs(moveX) > 0)
        {
            state = playerState.run;
        }
        else
        {
            state = playerState.idle;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = playerState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = playerState.fall;
        }

        if (moveJump || jumpHold && rb.velocity.y > 0f)
        {
            state = playerState.doublejump;
        }

        anim.SetInteger("states", (int)state);
    }

    public void UnlockDoubleJump()
    {
        doubleJumpUnlocked = true;
    }

}