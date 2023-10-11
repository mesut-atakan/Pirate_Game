using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;


public class CharacterConroller : ACharacterConroller
{

    Rigidbody2D rigidbody;
    BoxCollider2D collider;
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask groundLayer;

    bool gameStart;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        gameStart = true;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("I pres the d button");
            rigidbody.velocity = new Vector2(-BackWalkSpeed * RunSpeed * Time.deltaTime, rigidbody.velocity.y);
        }
        else
        {
            rigidbody.velocity = new Vector2(1f * RunSpeed * Time.deltaTime, rigidbody.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, JumpingPower);
        }

        if (Input.GetKeyDown(KeyCode.Space) && rigidbody.velocity.y > 0f)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * 0.5f);
        }
    }

    internal override bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, groundLayer);
    }
    internal override void Damage()
    {
        
    }

    internal override void Dead()
    {
        
    }

    internal override void Move()
    {
        
    }

    internal override void TakeDamage()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("GroundUp") && Input.GetKeyDown(KeyCode.DownArrow))
        {
            var box = collision.collider.gameObject.GetComponent<BoxCollider2D>();
            box.isTrigger = true;
            Debug.Log("I AM TRÝGGER");
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("GroundUp"))
        {
            var box = collision.collider.gameObject.GetComponent<BoxCollider2D>();
            box.isTrigger = false;
        }
    }
}
