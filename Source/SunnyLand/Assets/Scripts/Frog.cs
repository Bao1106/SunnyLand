
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    public Transform moveDetection;

    public Rigidbody2D frog;
    public Transform feetPos;
    public LayerMask whatIsGround;

    public float distance;

    private bool facingLeft = true;
    private bool isGround;
    private float jumpLength;
    private float jumpHeight;
    private float checkRadius;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        jumpLength = 3f;
        jumpHeight = 3f;
        checkRadius = 0.1f;

        //InvokeRepeating("Movement", 5f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        isGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        //Transition from Jump to Fall
        if (anim.GetBool("IsJump"))
        {
            if(frog.velocity.y < .1f)
            {
                anim.SetBool("IsFall", true);
                anim.SetBool("IsJump", false);
            }
        }

        //Transition from Fall to Idle
        if(isGround == true && anim.GetBool("IsFall"))
        {
            anim.SetBool("IsFall", false);
        }
    }

    public void Movement()
    {
        isGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        //transform.Translate(Vector2.left * jumpLength * Time.deltaTime);

        if (isGround == true)
        {
            frog.velocity = new Vector2(frog.velocity.x, jumpHeight);
            anim.SetBool("IsJump", true);
            //transform.Translate(Vector2.up * jumpHeight * Time.deltaTime);
        }

        RaycastHit2D moveInfo = Physics2D.Raycast(moveDetection.position, Vector2.down, distance);

        if(facingLeft)
        {
            frog.velocity = new Vector2(-jumpLength, frog.velocity.y);
            if (moveInfo.collider == false)
            {
                //transform.eulerAngles = new Vector2(0, 0);
                transform.localScale = new Vector2(-1, 1);
                facingLeft = false;
                frog.velocity = new Vector2(jumpLength, frog.velocity.y);
            }                      
        }
        else
        {
            frog.velocity = new Vector2(jumpLength, frog.velocity.y);
            if(moveInfo.collider == false)
            {
                //transform.eulerAngles = new Vector2(0, -180);
                transform.localScale = new Vector2(1, 1);
                facingLeft = true;
                frog.velocity = new Vector2(-jumpLength, frog.velocity.y);
            }            
        }
    }
   

    

}
