using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Shoot")]
    [SerializeField] float FireSpeed = 1f; // two attacks per/sec    
    [SerializeField] Transform firePosition;
    [SerializeField] float offset;
    [SerializeField] GameObject bulletPrefab;

    [Header("hitPoints")]
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] float durationCameraShake = 0.01f;
    [SerializeField] GameObject deathEffect;

    [Header("SFX")]
    [SerializeField] AudioClip shootSFX;
    [SerializeField] AudioClip deathSFX;
    
    CameraShake cameraShake;
    Transform player;
    AudioSource audioSource;

    float nextFireTime = 0;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraShake = GameObject.FindObjectOfType<CameraShake>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        LookAtPlayer();
        Shoot();
    }

    private void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            PlayShootSFX();
            GameObject bullet = Instantiate(bulletPrefab, firePosition.position, Quaternion.identity);
            nextFireTime = Time.time + 1f / FireSpeed;
        }
    }

    private void PlayParticleEffects()
    {
        GameObject impact = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(impact, 1f);

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartDeathSequence();
        }
    }

    private void StartDeathSequence()
    {
        //StartCoroutine(cameraShake.ShakeCamera(durationCameraShake));
        PlayDeathSFX();
        PlayParticleEffects();
        transform.tag = "Untagged";
        GetComponent<Collider2D>().enabled = false;
        //StopCoroutine(cameraShake.ShakeCamera(durationCameraShake));
        Destroy(gameObject);
        this.enabled = false;
    } 

    private void PlayDeathSFX()
    {
        audioSource.Stop();
        float randomPitch = UnityEngine.Random.Range(1.10f, 1.30f);
        audioSource.pitch = randomPitch;
        audioSource.PlayOneShot(deathSFX);
    }

    private void PlayShootSFX()
    {
        audioSource.pitch = 1.50f;
        audioSource.PlayOneShot(shootSFX);
    }

    public void LookAtPlayer()
    {
        Vector3 difference = player.position - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
    }
}
