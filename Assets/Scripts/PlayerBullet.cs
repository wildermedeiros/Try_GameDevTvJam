using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] int damage = 15;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] GameObject impactPrefab;

    void Update() 
    {
        transform.Translate(transform.right * bulletSpeed * Time.deltaTime);
    }

    // private void OnTriggerEnter2D(Collider2D other) 
    // {
    //     PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

    //     if (playerHealth != null)
    //     {
    //         playerHealth.TakeDamage(damage);
    //     }
    //     GameObject impact = Instantiate(impactPrefab, transform.position, Quaternion.identity);
    //     Destroy(impact, 1f);
    //     Destroy(gameObject);
    // }
}
