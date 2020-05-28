using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidBody2D;
    Transform player;
    AudioSource audioSource;
    CameraShake cameraShake;
    
    [Header("Health")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] float durationCameraShake = 0.3f;

    [Header("Attack")]
    [SerializeField] int attackDamage = 20;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackSpeed = 3;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform attackPoint;
    
    [SerializeField] ParticleSystem deathEffect;

    int currentHealth;
    float nextAttackTime = 0;
    bool isFlipped = false;


    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraShake = GameObject.FindObjectOfType<CameraShake>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            StartDeathSequence();
        }
    }

    private void StartDeathSequence()
    {
        animator.SetBool("Dead", true);
        StartCoroutine(cameraShake.ShakeCamera(durationCameraShake));
        PlaySoundEffects();
        PlayParticleEffects();
        transform.tag = "Untagged";
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    private void PlayParticleEffects()
    {
        deathEffect.Play();
    }

    private void PlaySoundEffects()
    {
        audioSource.Play();
    }

    // Event method
    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            Debug.Log("Enemy attack");
            Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
            if(hitPlayer != null)
            { 
                Debug.Log("Encontrou");
                hitPlayer.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + 1f / attackSpeed;
            //sound effect
            //vfx
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) { return; }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        
    }

    // animator behaviour method
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
