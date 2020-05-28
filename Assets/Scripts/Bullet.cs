using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigidBody2D;
    Transform player;

    [SerializeField] int damage = 15;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] GameObject impactPrefab;

    private void Awake() {
        rigidBody2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    { 
        rigidBody2D.velocity = -player.transform.position * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        GameObject impact = Instantiate(impactPrefab, transform.position, Quaternion.identity);
        Destroy(impact, 1f);
        Destroy(gameObject);

    }
}
