using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rigidBody2D;
    Transform player;
    Vector2 target;

    [SerializeField] int damage = 15;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] GameObject impactPrefab;

    private void Awake() {
        rigidBody2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        target = new Vector2(player.position.x, player.position.y);
    }

    void Update() 
    {
        Vector2 newPosition = Vector2.MoveTowards(rigidBody2D.position, target, bulletSpeed * Time.fixedDeltaTime);
        rigidBody2D.MovePosition(newPosition);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            GameObject impact = Instantiate(impactPrefab, transform.position, Quaternion.identity);
            Destroy(impact, 1f);
            Destroy(gameObject);
        }
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
