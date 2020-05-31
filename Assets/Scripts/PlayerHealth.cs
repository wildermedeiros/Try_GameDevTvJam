using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Animator animator;

    [SerializeField] CameraShake cameraShake;
    [SerializeField] HealthBar healthBar;

    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;
    [SerializeField] float durationCameraShake = 0.3f;
    
    bool isInvulnerable = false;
    bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if(isInvulnerable){ return; }

        if(isDead) {return; }

        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        healthBar.SetHealth(currentHealth);

        StartCoroutine(cameraShake.ShakeCamera(durationCameraShake));
        StartCoroutine(StopTimeInDamage());

        if (currentHealth <= 0)
        {
            StartDeathSequence();
        }
    }

    private void StartDeathSequence()
    {
        animator.SetBool("isDead", true);
        isDead = true;
        //PlaySoundEffects();
        //PlayParticleEffects();
        //GetComponent<Collider2D>().enabled = false;
        this.enabled = false;      
    }

    IEnumerator StopTimeInDamage()
    {
        Time.timeScale = 0f; 

        yield return new WaitForSecondsRealtime(0.1f);

        Time.timeScale = 1;
    }

    void OnDashing(bool modifier) // string reference
    {
        isInvulnerable = modifier;
    }
}
