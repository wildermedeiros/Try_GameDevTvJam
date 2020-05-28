using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunBehaviour : StateMachineBehaviour
{
    Enemy enemy;
    Transform player;
    Rigidbody2D rigidBody2D;
    [SerializeField] float movimentSpeed = 5f;
    [SerializeField] float attackRange = 3f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidBody2D = animator.GetComponent<Rigidbody2D>();
        enemy = animator.GetComponent<Enemy>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, rigidBody2D.position.y);
        Vector2 newPosition = Vector2.MoveTowards(rigidBody2D.position, target, movimentSpeed * Time.fixedDeltaTime);
        rigidBody2D.MovePosition(newPosition);

        if(Vector2.Distance(player.position, rigidBody2D.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }

        // TODO maybe putting some behaviour here, about the player attack range and other stuffs
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
