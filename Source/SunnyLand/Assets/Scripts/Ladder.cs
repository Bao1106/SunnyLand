using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private float vertical;
    private float climbSpeed = 6.5f;
    private bool isLadder;
    private bool isClimb;

    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        if(isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimb = true;
        }
    }

    private void FixedUpdate()
    {
        if (isClimb)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);
        }
        else
        {
            rb.gravityScale = 3f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimb = false;
        }
    }
}
