using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    //PlayerInputActions playerInputActions;
    AudioSource audioSource;

    [SerializeField] float offset;

    [Header("Ranged Attack")]
    [SerializeField] float FireSpeed = 1f; // two attacks per/sec   
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePosition;
    [SerializeField] GameObject[] guns;

    float nextFireTime = 0;

    //InputAction rangedAttack;

    private void Awake() 
    {
        //playerInputActions = new PlayerInputActions();
        //rangedAttack = playerInputActions.PlayerControls.RangedAttack;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(2))
        {
            SetGunsActive(true);
            //PlaySoundEffect();
        }
        else
        {
            SetGunsActive(false);
            audioSource.Stop();
        }
    }

    private void SetGunsActive(bool isActive)
    {
        foreach (GameObject gun in guns)
        {
            var emissionModule = gun.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
