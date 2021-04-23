using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigMovement : MonoBehaviour
{
	#region Variabes
	[SerializeField] GameManager manager;
	[SerializeField] PigEquips pig;
	[SerializeField] Animator animPig;
	[SerializeField] Rigidbody2D rbpig;
	[SerializeField] Transform attackPoint;
	[SerializeField] Transform groundCheck;
	[SerializeField] LayerMask whatIsGround;

	public LayerMask whoIsPlayer;

	public float groundRadius = 0.01f;
	public float rangeOfDetect = 0.8f;
	float attackRange = 0.1f;
	float damage = 20f;

	bool isFlipped = false;
	bool isGrounded = true;
    #endregion

    #region BuiltInFunction

    private void Update()
    {
		if(!pig.playerIsDead)
        {
			RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Vector2.left, rangeOfDetect, whoIsPlayer);
			RaycastHit2D rayInfo2 = Physics2D.Raycast(transform.position, Vector2.right, rangeOfDetect, whoIsPlayer);
			if (rayInfo || rayInfo2)
			{
				animPig.SetBool("run", true);
			}
		}
		else
        {
			StartCoroutine(Dead());
		}
		if (rbpig.velocity.x > 0f && !isFlipped)
		{
			pig.Flip();
		}
		else if (rbpig.velocity.x < 0f && isFlipped)
		{
			pig.Flip();
		}
	}
    private void FixedUpdate()
    {
		if (Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround))
		{
			isGrounded = true;
			animPig.SetBool("grounded", isGrounded);
		}
        else
        {
			isGrounded = false;
			animPig.SetBool("grounded", isGrounded);
        }
	}
    #endregion

    #region Custom Methods
    public void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;
		if (manager.player != null)
        {
			if (transform.position.x > manager.player.transform.position.x && isFlipped)
			{
				transform.localScale = flipped;
				transform.Rotate(0f, 180f, 0f);
				isFlipped = false;
			}
			else if (transform.position.x < manager.player.transform.position.x && !isFlipped)
			{
				transform.localScale = flipped;
				transform.Rotate(0f, 180f, 0f);
				isFlipped = true;
			}
		}
	}
	IEnumerator Dead()
    {
		yield return new WaitForSeconds(2f);
		pig.GetComponent<Rigidbody2D>().isKinematic = true;
		pig.GetComponent<CapsuleCollider2D>().enabled = false;
		pig.GetComponent<SpriteRenderer>().enabled = false;
		if(manager.killedAll)
        {
			manager.OpenDoor();
        }
	}
	public void Attack()
    {
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, whoIsPlayer);
		foreach (Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<CharacterEquips>().DamagePig(damage);
		}
	}
    #endregion
}
