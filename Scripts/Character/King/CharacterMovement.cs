using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterMovement : MonoBehaviour
{
    #region Variables 

    [SerializeField] Rigidbody2D rigidBody2d;  // Taking the reference of rigidbody attached to the character
    [SerializeField] Animator anim;     // Taking the animator controller for playing the animations
    [SerializeField] LayerMask whatIsGround;  // Create a layer mask for checking the player is on ground or not
    [SerializeField] LayerMask whoisenemy;  // Create a layer mask to detect the enemy layer set on the enemies
    [SerializeField] Transform groundCheck;   // Take a empty gameobject placed under the foot of the character to check the ground and position of the character
    [SerializeField] Transform attackPoint; //Take a attack point and create a range of attack and damage to the enemies
    [SerializeField] CharacterEquips equips;
    [SerializeField] GameManager manager;
    [SerializeField] LayerMask lastDoor;
    
    float moveX = 0f;
    float runSpeed = 65f;
    float jumpForce = 200f;
    float groundRadius = 0.01f;
    float attackRange = 0.22f;
    float attackRate = 2f;
    float nextAttackTime = 0f;

    bool jumpY = false;
    bool faceRight = true;
    bool isGrounded = false;
    public bool doMove = false;
    #endregion

    #region Builtin Methods
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        //Take input of horizontal axis for right-left movement
        //and button input for jumping with jump button
        if(doMove && !equips.playerIsDead)
        {
            moveX = CrossPlatformInputManager.GetAxis("Horizontal") * runSpeed;
            if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded)
            {
                jumpY = true;
                anim.SetBool("on_jump", true);
            }
            if (Time.time >= nextAttackTime)
            {
                if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                {
                    Attack();
                    nextAttackTime = Time.time + (1.5f / attackRate);
                }
            }
        }
        if(equips.playerIsDead)
        {
            rigidBody2d.constraints = RigidbodyConstraints2D.FreezePositionX;
            rigidBody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void FixedUpdate()
    {
        // Use fixed Update for the physics
        // Move the Character here
        if (Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround))
        {
            isGrounded = true;
            anim.SetBool("on_jump", false);
            anim.SetBool("is_grounded", isGrounded);
        }
        else
        {
            isGrounded = false;
        }
        Move(moveX * Time.fixedDeltaTime, jumpY);

        if(!isGrounded)
        {
            anim.SetFloat("do_fall", rigidBody2d.velocity.y);
        }
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.2f, lastDoor);
        if (col)
        {
            manager.inTouch = true;
        }
        else if(!col)
        {
            manager.inTouch = false;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Diamond"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            manager.DiamontCount();
        }
        if(collision.gameObject.CompareTag("Health"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if (equips.totalLives == 3)
            {
                equips.totalLives = 3;
            }
            else
            {
                equips.totalLives += 1;
            }
            manager.HealthManager(equips.totalLives);
        }
        if(collision.gameObject.CompareTag("Spikes"))
        {
            equips.Die();
        }
    }
    #endregion

    #region Custom Methods
    private void Move(float horizontalSpeed, bool jump)
    {
        if(!equips.playerIsDead)
        {
            //check is the player is grounded and is jumping then add force for jump
            if (isGrounded && jump)
            {
                isGrounded = false;
                jumpY = false;
                rigidBody2d.AddForce(new Vector2(0f, jumpForce));
            }

            //Flip the sprite as character moves left or right
            if (horizontalSpeed > 0f && !faceRight)
            {
                Flip();
                equips.Flip();
            }
            else if (horizontalSpeed < 0f && faceRight)
            {
                Flip();
                equips.Flip();
            }

            rigidBody2d.velocity = new Vector2(horizontalSpeed, rigidBody2d.velocity.y);

            if (horizontalSpeed < 0f && isGrounded)
            {
                anim.SetFloat("do_run", rigidBody2d.velocity.x * (-1));
            }
            else if (isGrounded)
            {
                anim.SetFloat("do_run", rigidBody2d.velocity.x);
            }
        }

    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        faceRight = !faceRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Attack()
    {
        //Play Animation and detetct the enemies in range of attack
        anim.SetTrigger("on_attack");
        Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(attackPoint.position, attackRange, whoisenemy);
        foreach(Collider2D enemy in hitEnemies)
        {
            if(enemy.gameObject.CompareTag("Enemy"))
            {
                enemy.GetComponent<PigEquips>().Damage();
            }
            else if(enemy.gameObject.CompareTag("Box"))
            {
                enemy.GetComponent<Box>().OnHit();
            }
        }
    }
    #endregion
}
