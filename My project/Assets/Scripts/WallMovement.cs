using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    public Vector3 wallOffsets;
    public bool isLeftWall, isRightWall;
    public LayerMask wallLayer;
    public bool isWallMove;
    public bool canMove = true;
    public enum WallState
    {
        wallGrab,
        wallSlide,
        wallClimb,
        wallJump,
        none
    }

    WallState ws;

    Rigidbody2D rb;

    private bool wallUnlocked = false;
    public GameObject wallImgae;


    // Start is called before the first frame update
    void Start()
    {
        ws = WallState.none;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wallUnlocked)
        {
            wallImgae.SetActive(true);
            float playerInput = Input.GetAxis("Vertical");

            WallCheck();

            //if (isLeftWall || isRightWall)
            //{
            //    if (isWallMove && Input.GetButtonDown("Jump"))
            //    {
            //        //wallJump
            //        WallJump();
            //    }
            //    else if (Input.GetButtonDown("Jump"))
            //    {
            //        Jump(Vector2.up);
            //    }
            //}


            if (Input.GetKey(KeyCode.W) && (isLeftWall || isRightWall))
            {
                isWallMove = true;
            }
            else
            {
                isWallMove = false;
            }


            if (isWallMove && ws != WallState.wallJump)
            {
                rb.gravityScale = 0f;

                //wallmovement
                if (playerInput > 0)
                {
                    //wallclimb
                    WallClimb();
                }

            }
            else
            {
                ws = WallState.none;
                rb.gravityScale = 3f;
            }

            if (canMove)
            {
                Movement();
            }
        }
        else
        {
            wallImgae.SetActive(false);
        }
        
    }

    void WallCheck()
    {
        isLeftWall = Physics2D.OverlapCircle(transform.position - wallOffsets, 0.1f, wallLayer);
        isRightWall = Physics2D.OverlapCircle(transform.position + wallOffsets, 0.1f, wallLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position - wallOffsets, 0.1f);
        Gizmos.DrawWireSphere(transform.position + wallOffsets, 0.1f);
    }

    void WallGrab()
    {
        rb.velocity = Vector2.zero;
        ws = WallState.wallGrab;
    }

    void WallClimb()
    {
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(rb.velocity.x, 5);
        ws = WallState.wallClimb;
    }

    void WallSlide()
    {
        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(rb.velocity.x, -7);
        ws = WallState.wallSlide;
    }

    void WallJump()
    {
        StartCoroutine(DisableMovement());
        Vector3 dir = isRightWall ? Vector3.left : Vector3.right;
        Jump(Vector3.up + dir / 2f);
        ws = WallState.wallJump;
        rb.gravityScale = 3;
    }

    IEnumerator DisableMovement()
    {
        canMove = false;
        yield return new WaitForSeconds(0.3f);
        canMove = true;
    }

    void Jump(Vector2 direction)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(direction * 4, ForceMode2D.Impulse);
    }

    void Movement()
    {
        float playerInput = Input.GetAxis("Horizontal");
        if (playerInput != 0 && !isWallMove)
        {
            rb.velocity = new Vector2(playerInput * 5, rb.velocity.y);
        }
    }

    public void UnlockWallJump()
    {
        wallUnlocked = true;
    }

}
