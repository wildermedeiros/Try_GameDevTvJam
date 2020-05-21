using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;

    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
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
}
