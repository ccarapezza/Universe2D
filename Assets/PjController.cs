using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PjController : MonoBehaviour {

    public float speed;
    public float jumpForce;
    public float groundChekerDistance = 0.01f;
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    BoxCollider2D bodyCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bodyCollider = GetComponent<BoxCollider2D>();
    }

    public bool Walk {
        get {
            return animator.GetBool("Walk"); ;
        }
        set
        {
            animator.SetBool("Walk", value);
        }
    }

    bool IsInGround() {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector3.down, groundChekerDistance);
        Debug.DrawRay(transform.position, Vector3.down * groundChekerDistance);
        if (raycastHit.collider!=null)
            return true;
        else
            return false;
    }

    void Start () {
		
	}

	void Update () {
        animator.SetFloat("VerticalVelocity", rb.velocity.y);
        animator.SetBool("InGround", IsInGround());

        print("Ground: " + IsInGround());

        if (Walk)
            spriteRenderer.flipX = Input.GetAxis("Horizontal") < 0;

        bodyCollider.size = spriteRenderer.bounds.size;
        bodyCollider.offset = new Vector2(0 , spriteRenderer.bounds.size.y * 0.5f);
    }

    private void FixedUpdate()
    {
        Walk = Input.GetAxis("Horizontal") != 0;
        transform.position += Vector3.right * speed * Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space) && IsInGround())
            rb.AddForce(Vector2.up * jumpForce);
    }
}
