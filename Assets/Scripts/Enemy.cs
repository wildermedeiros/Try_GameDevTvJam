using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidBody2D;
    Transform player;

    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int maxHealth = 100;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackRate = 3;

    int currentHealth;
    float nextAttackTime = 0;

    [SerializeField] bool isFlipped = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        animator.SetTrigger("Dead");
        //display soundefx
        //display vfx ?
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    public void Attack()
    {
        Debug.Log("Enemy attack");
        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (var enemy in hitEnemies)
            {
                //enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + 1f / attackRate;
            //sound effect 
            //vfx
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) { return; }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        //Gizmos.DrawWireSphere(transform.position, attackRange);
        
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
