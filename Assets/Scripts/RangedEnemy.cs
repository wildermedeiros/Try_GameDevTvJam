using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float stoppingDistance = 2f;
    [SerializeField] float retreatDistance = 5f;        
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float FireSpeed = 1f; // two attacks per/sec    
    [SerializeField] Transform firePosition;

    Transform player;
    Rigidbody2D rigidBody2D;
    Animator animator;

    float nextFireTime = 0;
    bool isFlipped = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        LookAtPlayer();

        var distanceToTarget = Vector2.Distance(transform.position, player.position);

        if (distanceToTarget > stoppingDistance)
        {
            ChaseTarget();
        }
        else if (distanceToTarget < stoppingDistance && distanceToTarget > retreatDistance)
        {
            StopChasing();
        }
        else if (distanceToTarget < retreatDistance)
        {
            //RetreatFromTarget();
        }

        Shoot();
    }

    private void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            animator.SetTrigger("Attack");
            GameObject bullet = Instantiate(bulletPrefab, firePosition.position, Quaternion.identity);
            nextFireTime = Time.time + 1f / FireSpeed;
        }
    }

    private void RetreatFromTarget()
    {
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        Vector2 newPosition = Vector2.MoveTowards(transform.position, target, -moveSpeed * Time.fixedDeltaTime);
        rigidBody2D.MovePosition(newPosition);
    }

    private void StopChasing()
    {
        animator.SetBool("Running", false);
        transform.position = this.transform.position;
    }

    private void ChaseTarget()
    {
        animator.SetBool("Running", true);
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        Vector2 newPosition = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime);
        rigidBody2D.MovePosition(newPosition);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
}
