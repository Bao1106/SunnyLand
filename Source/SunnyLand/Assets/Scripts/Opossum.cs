using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : Enemy
{
    public Transform pos1, pos2;
    public Transform startPos;

    public Rigidbody2D rat;    

    private float speed;
    public bool facingLeft = true;

    Vector2 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        speed = 4f;
        nextPos = startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == pos1.position)
        {
            if (transform.localScale.x != 1)
            {
                transform.localScale = new Vector2(1, 1);
            }
            nextPos = pos2.position;
        }

        if (transform.position == pos2.position)
        {
            if (transform.localScale.x != -1)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            nextPos = pos1.position;
        }

        transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
