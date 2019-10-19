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

    protected virtual void Start()
    {
        gravity = 0.9f;
        sr = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        vCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
        body.gravityScale = gravity;
        body.drag = 0f;
        sr.flipX = false;
        bool grounded = Grounded();

        // can't move if transition exists
        int hDirection = 0;
        if (Input.GetKey("left") && Movable())
        {
            hDirection--;
        }
        if (Input.GetKey("right") && Movable())
        {
            hDirection++;
        }
        body.velocity = new Vector2(hDirection * horizontalSpeed, body.velocity.y);

        if (Input.GetKeyDown("up") && Jumpable())
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
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
            vCollider.bounds.extents.y - vCollider.offset.y + 0.1f);
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

}