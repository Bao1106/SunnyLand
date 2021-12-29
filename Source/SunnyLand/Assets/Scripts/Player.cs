using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D player;
    public Animator playerAnim;   

    public Transform feetPos;
    public LayerMask whatIsGround;

    //For Ladder
    public bool canClimb = false;

    float crouchSpeed = 2.5f;

    private enum State { Idle, Running, Jumping, Falling, Hurt, Crouch, Climb }
    private State state = State.Idle;

    [SerializeField] private float speed;
    private float checkRadius;
    private float jumpTime;
    private float jumpForce;
    private float moveInput;
    private float jumpCounter;
    private float checkSpeed; // Use to check speed before crouch
    private bool isCrouch;
    private bool isGround;
    private bool checkLadder;

    private int currentHealth; // Use to save health before eat power up or taking damage

    [SerializeField] Collider2D standingCollider;
    [SerializeField] Collider2D crouchingCollider;

    [SerializeField] private float damageDeal = 15f;
    [SerializeField] private int Cherries = 0;
    [SerializeField] private int Health;
    [SerializeField] private TextMeshProUGUI cherriesText;
    [SerializeField] private Text HealthAmount;
    [SerializeField] private AudioSource cherrySound;
    [SerializeField] private AudioSource footStep;
    [SerializeField] private AudioSource gemSound;
    [SerializeField] private AudioSource Background;
    [SerializeField] private AudioSource playerHurt;

    private void StateSwitch()
    {
        if(checkLadder && Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            state = State.Climb;
        }
        else if(state == State.Jumping)
        {
            if(player.velocity.y < .1f)
            {
                state = State.Falling;
            }
        }
        else if(state == State.Falling)
        {
            if (isGround == true)
            {
                state = State.Idle;
            }
           
        }
        else if (state == State.Hurt)
        {
            if (Mathf.Abs(player.velocity.x) < .1f)
            {
                state = State.Idle;
            }
        }
        else if (Mathf.Abs(player.velocity.x) > 2f) 
        {
            state = State.Running;
            if(isCrouch == true)
            {
                state = State.Crouch;
            }
        }
        else
        {
            state = State.Idle;
            if (isCrouch == true)
            {
                state = State.Crouch;
            }
        }
        playerAnim.SetInteger("State", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {            
            cherrySound.Play();
            Destroy(collision.gameObject);
            Cherries += 1;
            cherriesText.text = Cherries.ToString();
        }

        if (collision.tag == "PowerUp")
        {
            gemSound.Play();
            Destroy(collision.gameObject);
            speed = 10f;
            checkSpeed = speed;
            crouchSpeed = 7.5f;
            Health += 50;
            HealthAmount.text = Health.ToString();            
        }

        if (collision.CompareTag("Ladder"))
        {
            checkLadder = true;
        }

        if(collision.tag == "Death")
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            checkLadder = false;
        }
    }

    private void ResetStat()
    {
        speed = 5f;
        checkSpeed = speed;
        crouchSpeed = 2.5f;
        Health = currentHealth;
        HealthAmount.text = Health.ToString();
    }

    //private void Hurt()
    //{
    //    playerHurt.Play();
    //}

    private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if(other.gameObject.tag == "Enemy")
        {
            if(state == State.Falling)
            {
                enemy.Death();
                jumpCounter = 1;
                Jump();
            }
            else
            {
                state = State.Hurt;
                LifeForce();
                playerHurt.Play();
                //Hurt();

                if(other.gameObject.transform.position.x > transform.position.x)
                {
                    // Enemy is to right and damaged player
                    player.velocity = new Vector2(-damageDeal, player.velocity.y); 
                }
                else
                {
                    // Enemy is to left and damaged player
                    player.velocity = new Vector2(damageDeal, player.velocity.y);
                }
            }
            
        }

        if (other.gameObject.tag == "Spike")
        {
            state = State.Hurt;
            LifeForce();
            playerHurt.Play();

            if (other.gameObject.transform.position.x > transform.position.x)
            {
                // Enemy is to right and damaged player
                player.velocity = new Vector2(-damageDeal, player.velocity.y);
            }
            else
            {
                // Enemy is to left and damaged player
                player.velocity = new Vector2(damageDeal, player.velocity.y);
            }

        }

        if (other.gameObject.tag == "Fire")
        {
            state = State.Hurt;
            FireDealDamage();
            playerHurt.Play();

            if (other.gameObject.transform.position.x > transform.position.x)
            {
                // Enemy is to right and damaged player
                player.velocity = new Vector2(-damageDeal, player.velocity.y);
            }
            else
            {
                // Enemy is to left and damaged player
                player.velocity = new Vector2(damageDeal, player.velocity.y);
            }

        }

    }

    private void LifeForce()
    {
        Health -= 10;
        currentHealth = Health;
        HealthAmount.text = Health.ToString();
        if (Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void FireDealDamage()
    {
        Health -= 20;
        currentHealth = Health;
        HealthAmount.text = Health.ToString();
        if(Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void FootStep()
    {
        footStep.Play();
    }

    private void checkMovement()
    {
        speed = checkSpeed;
    }

    private void checkBoostMove()
    {
        speed = 10f;
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = 5f;
        checkSpeed = speed;

        jumpForce = 13f;
        jumpTime = 1f;
        checkRadius = 0.5f;
        jumpCounter = jumpTime;
        Health = 50;
        currentHealth = Health;
        HealthAmount.text = Health.ToString();
        //Background.Play();
    }

    // Update is called once per frame
    void Update()
    {
        isGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        standingCollider.enabled = true;
        crouchingCollider.enabled = false;

        if (isGround == true)
        {
            jumpCounter = jumpTime;
            standingCollider.enabled = !isCrouch;
            crouchingCollider.enabled = isCrouch;
        }

        if (state == State.Climb)
        {
            Climb();
        }

        if (state != State.Hurt) 
        {
            Move();
        }
        
        StateSwitch();
        Invoke("ResetStat", 30);
               
    }

    public void Move()
    {
        //canClimb = ladder.isClimb;
        moveInput = Input.GetAxis("Horizontal");

        if (isCrouch == true)
        {
            //player.velocity = new Vector2(moveInput * crouchSpeed, player.velocity.y);
            speed = crouchSpeed;
        }

        player.velocity = new Vector2(moveInput * speed, player.velocity.y);       

        if (moveInput < 0) //moveInput < 0
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (moveInput > 0) //moveInput > 0
        {
            transform.localScale = new Vector2(1, 1);
        }
        Jump();
        Crouch();
    }

    public void Climb()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            player.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter > 0)
        {
            player.velocity = Vector2.up * jumpForce;
            jumpCounter--;
            state = State.Jumping;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && jumpCounter == 0 && isGround == true)
        {
            player.velocity = Vector2.up * jumpForce;
            state = State.Jumping;
        }
    }

    public void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            isCrouch = false;
            if(checkSpeed == 10f)
            {
                checkBoostMove();
            }
            checkMovement();
        }
    }

}
