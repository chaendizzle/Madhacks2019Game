using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalSpeed;
    public float jumpSpeed;
    private readonly float MAXFALLSPEED = -10;
    private bool squat;
    private readonly int SQUATLENGTH = 3;
    private int squatCount;
    private float gravity;
    private Sprite[] gasSprites;

    protected Animator animator;
    protected Rigidbody2D body;
    public SpriteRenderer sr;
    protected Collider2D vCollider;

 	[HideInInspector] public int facingX = -1;

    // border that the player must stay within the camera
    public float borderX = 1f;
    public float borderY = 1f;
    PlayerInput playerInput;

    protected virtual void Start()
    {
        gravity = 5.5f;
        sr = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        vCollider = GetComponent<CapsuleCollider2D>();
        playerInput = GetComponent<PlayerInput>();
        sr.flipX = false;
        squatCount = 0;
        squat = false;
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
        playerInput.UpdateInput();
        body.gravityScale = gravity;
        body.drag = 0f;
        bool grounded = Grounded();

        if (squat)
        {
            squatCount++;
            //Debug.Log(squatCount);
        }

        //Tests Grounded()
        //if (grounded)
        //{
        //    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        //}
        //else
        //{
        //    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
        //}

        int hDirection = (playerInput.xmovement < 0 && Movable()) ? -1 : 
                            (playerInput.xmovement > 0 && Movable()) ? 1 : 0;


        float windSpeed = 0f;
        if (ClimateEvents.GetInstance().wind && !grounded)
        {
            windSpeed = -3f;
        }
        Director.GetInstance().xVelocity = Mathf.Abs(horizontalSpeed) * playerInput.xmovement + GetCameraSpeed() * 0.6f + windSpeed;
        if (hDirection < 0)
        {
            body.velocity = new Vector2(horizontalSpeed * playerInput.xmovement + GetCameraSpeed() * 0.6f + windSpeed, body.velocity.y);
        }
        else if (hDirection > 0)
        {
            body.velocity = new Vector2(horizontalSpeed * playerInput.xmovement + GetCameraSpeed() * 0.6f + windSpeed, body.velocity.y);
        }
        else
        {
            if (grounded)
            {
                body.velocity = new Vector2(0, body.velocity.y);
            }
            else
            {
                body.velocity = new Vector2(GetCameraSpeed() * 0.6f, body.velocity.y);
            }
        }

        if (playerInput.up && Jumpable())
        {
            animator.SetTrigger("Jumpsquat");
            if(!squat)
                squat = true;
        }
        if (playerInput.up)
        {
            body.gravityScale = gravity * 0.5f;
        }


        if (squatCount == SQUATLENGTH)
        {
            squat = false;
            squatCount = 0;
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);

            // Change jump height based on if key is held
            //if (Input.GetKeyDown("up"))
            //{
            //    body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            //}
            //else
            //{
            //    body.velocity = new Vector2(body.velocity.x, (int) 0.5 * jumpSpeed);
            //}
        }


        // animate with blend tree
        //animator.SetFloat("VelocityX", hDirection);
        if (hDirection != 0)
        {
            //animator.SetFloat("FacingX", hDirection);
            facingX = hDirection;
        }
        //animator.SetFloat("VelocityY", Mathf.Sign(body.velocity.y));
        //animator.SetBool("Grounded", grounded);

        if (hDirection > 0)
        {
            sr.flipX = false;
        }
        else if (hDirection < 0)
        {
            sr.flipX = true;
        }

        if (body.velocity.y < MAXFALLSPEED)
        {
            body.velocity = new Vector2(body.velocity.x, MAXFALLSPEED);
        }


        if (transform.position.x > CameraMovement.CameraRect().xMax)
        {
            transform.position = new Vector3(CameraMovement.CameraRect().xMax, transform.position.y, transform.position.z);
        }

        // Set all animation indicators
        animator.SetFloat("VelocityX", body.velocity.x);
        animator.SetFloat("VelocityY", body.velocity.y);
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Run", (body.velocity.x > 0 || body.velocity.x < 0) && grounded);
    }

    protected virtual bool Jumpable()
    {
        return Grounded();
    }

    protected virtual bool Movable()
    {
        return true;
    }

    public bool Grounded()
    {
        return CheckRaycastGround(Vector2.zero) ||
            CheckRaycastGround(Vector2.left * (vCollider.bounds.extents.x + vCollider.offset.x)) ||
            CheckRaycastGround(Vector2.right * (vCollider.bounds.extents.x + vCollider.offset.x));
    }

    private bool CheckRaycastGround(Vector2 pos)
    {
        // player has 2 colliders, so to find more, make this 3
        RaycastHit2D[] results = new RaycastHit2D[3];
        // raycast for a collision down
        Physics2D.Raycast((Vector2)transform.position + pos, Vector2.down, new ContactFilter2D(), results,
            vCollider.bounds.extents.y + vCollider.offset.y + 0.3f);
        // make sure raycast hit isn't only player
        foreach (RaycastHit2D result in results)
        {
            if (result.collider != null)
            {
                if (result.collider.gameObject.tag != "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    private float GetCameraSpeed()
    {
        return Camera.main.gameObject.GetComponent<CameraMovement>().speed.x;
    }
}