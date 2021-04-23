using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigRun : StateMachineBehaviour
{
    GameManager manager;
    Transform player;
    Rigidbody2D pigRB2D;
    PigMovement pig;

    float speed = 0.6f;
    float attackRange = 0.2f;
    float nextAttackTime = 0f;
    float attackRate = 2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if(manager.player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        pigRB2D = animator.GetComponent<Rigidbody2D>();
        pig = animator.GetComponent<PigMovement>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Collider2D col = Physics2D.OverlapCircle(pig.transform.position, pig.rangeOfDetect, pig.whoIsPlayer);
        if(col)
        {
            pig.LookAtPlayer();
            Vector2 target = new Vector2(player.position.x, pigRB2D.position.y);
            Vector2 newPosition = Vector2.MoveTowards(pigRB2D.position, target, speed * Time.fixedDeltaTime);
            pigRB2D.MovePosition(newPosition);
            if(Vector2.Distance(player.position, pigRB2D.position) <= attackRange)
            {
                if(Time.time >= nextAttackTime)
                {
                    animator.SetTrigger("attack");
                    nextAttackTime = Time.time + (2f / attackRate);
                }
            }
        }
        else
        {
            animator.SetBool("run", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("attack");
    }
}
