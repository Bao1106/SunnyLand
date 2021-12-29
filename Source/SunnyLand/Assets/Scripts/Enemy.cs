using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D enemy;
    protected AudioSource death;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        enemy = GetComponent<Rigidbody2D>();
        death = GetComponent<AudioSource>();
    }

    public void Death()
    {
        anim.SetTrigger("Death");
        death.Play();
        enemy.velocity = Vector2.zero;
    }

    public void ClearAnim()
    {
        Destroy(this.gameObject);
    }
}
