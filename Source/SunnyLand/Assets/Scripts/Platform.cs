using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform Pos1, Pos2;
    public Transform startPos;

    private float speed;

    Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2f;
        nextPos = startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position == Pos1.position)
        {
            nextPos = Pos2.position;
        }

        if (transform.position == Pos2.position)
        {
            nextPos = Pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Pos1.position, Pos2.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
