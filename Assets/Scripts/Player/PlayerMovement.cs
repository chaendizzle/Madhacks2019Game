using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalSpeed;
    public float jumpSpeed;
    private float gravity;
    private Sprite[] gasSprites;

    protected Animator animator;
    protected Rigidbody2D body;
    protected SpriteRenderer sr;
    protected Collider2D vCollider;

 	[HideInInspector] public int facingX = -1;

    // border that the player must stay within the camera
    public float borderX = 1f;
    public float borderY = 1f;

    protected virtual void Start()
    {
        gravity = 0.9f;
        sr = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        vCollider = GetComponent<CapsuleCollider2D>();
        sr.flipX = false;
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
        body.gravityScale = gravity;
        body.drag = 0f;
        bool grounded = Grounded();

        //Tests Grounded()
        //if (grounded)
        //{
        //    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        //}
        //else
        //{
        //    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
        //}

        int hDirection = (Input.GetKey("left") && !Input.GetKey("right") && Movable()) ? -1 : 
                            (!Input.GetKey("left") && Input.GetKey("right") && Movable()) ? 1 : 0;


        if (hDirection < 0)
        {
            body.velocity = new Vector2(-horizontalSpeed + GetCameraSpeed() * 0.6f, body.velocity.y);
        }
        else if (hDirection > 0)
        {
            body.velocity = new Vector2(horizontalSpeed + GetCameraSpeed() * 0.6f, body.velocity.y);
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
        

        if (Input.GetKeyDown("up") && Jumpable())
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            animator.SetTrigger("Jumpsquat");
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

    protected bool Grounded()
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
            vCollider.bounds.extents.y + vCollider.offset.y + 0.1f);
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