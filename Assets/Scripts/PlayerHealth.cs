using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerHealth : MonoBehaviour
{
    Animator animator;

    [SerializeField] CameraShake cameraShake;
    [SerializeField] HealthBar healthBar;

    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;
    [SerializeField] float durationCameraShake = 0.3f;

    [SerializeField] PlayableDirector endSequence;

    float searchCountdown = 1f;
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
        endSequence.Play();
        //PlaySoundEffects();
        //PlayParticleEffects();
        //GetComponent<Collider2D>().enabled = false;
        //this.enabled = false;
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

    public void SearchForEnemies()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemys)
            {
                enemy.SetActive(false);
            }
        }
    }
}
