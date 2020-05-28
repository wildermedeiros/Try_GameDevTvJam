using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    Transform player;

    // TODO implementar um fire position
    [SerializeField] float range = 20f;
    [SerializeField] LayerMask playerMask;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float FireSpeed = 1f; // two attacks per/sec
    [SerializeField] int attackRange = 50;

    float nextFireTime = 0;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //ProcessRayCast();

        //transform.LookAt();
        ScanningForTarget();
    }

    private void ProcessRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, range, playerMask);
        if (hit)
        {
            //Debug.Log(hit.transform.name);
            //CreateHitImpact(hit);

            if(Time.time >= nextFireTime)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
                nextFireTime = Time.time + 1f / FireSpeed;
            } 
        }
        else
        {
            return;
        }
    }

    void ScanningForTarget(){
        if (Vector2.Distance(player.position, transform.position) <= attackRange)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            nextFireTime = Time.time + 1f / FireSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //if (attackPoint == null) { return; }

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // private void CreateHitImpact(RaycastHit hit)
    // {
    //     if (!PauseHandler.gameIsPaused)
    //     {
    //         GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
    //         Destroy(impact, 1f);
    //     }
    // }
}
