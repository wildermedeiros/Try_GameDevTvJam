using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumpController : MonoBehaviour
{
    [SerializeField] float fallMultiplayer = 2.5f;

    Rigidbody2D rigidBody2D;

    private void Awake() {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rigidBody2D.velocity.y < 0)
        {
            rigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplayer - 1) * Time.deltaTime;
        }
    }
}
