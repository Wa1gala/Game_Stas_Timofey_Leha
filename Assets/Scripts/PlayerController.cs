using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D rb;

    private bool facingRight = true;

    private bool isGrounde;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public static bool checkDie = true;
    public static bool checkTurn = false;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
    }

    public static bool checkDieValue
    {
        get { return checkDie; }
    }      
    public static bool _checkTurn
    {
        get { return checkTurn; }
    }  

    private void FixedUpdate()
    {
        //float distance = speed * Time.deltaTime * Input.GetAxis("Horizontal");
        //transform.Translate(Vector3.right * distance);


        moveInput = Input.GetAxis("Horizontal");
        if(checkDieValue)
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        if (facingRight == false && moveInput > 0 && checkDieValue)
        {
            Flip();
            checkTurn = false;
        }
        else if (facingRight == true && moveInput < 0 && checkDieValue)
        {
            Flip();
            checkTurn = true;
        }

        if (moveInput == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
    }

    private void Update()
    {
        isGrounde = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounde == true && Input.GetKeyDown(KeyCode.Space) && checkDieValue)
        {
            rb.velocity = Vector2.up * jumpForce;
            anim.SetTrigger("takeOf");
        }

        if (isGrounde == true)
        {
            anim.SetBool("isJumping", false);
        }
        else
        {
            anim.SetBool("isJumping", true);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}