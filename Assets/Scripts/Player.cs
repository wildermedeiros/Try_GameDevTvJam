using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    Rigidbody2D rigidBody2D;
    Animator animator;
    Collider2D myCapsuleCollider2D;

    State state; 

    enum State{
        Normal, 
        Attacking,
    }

    [Header("General")]
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;

    [Header("Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] float attackRange = 2f;
    [SerializeField] int attackDamage = 40;
    [SerializeField] float attackRate = 2f; // two attacks per/sec

    [Header("Dash")]
    [SerializeField] float startDashTime = 0.1f;
    [SerializeField] float dashSpeed = 50f;

    float direction = 0;
    float dashTime = 0;
    float nextAttackTime = 0;
    Vector2 movementInput;
    InputAction dash;
    InputAction attack;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerControls.Movement.performed += context => movementInput = context.ReadValue<Vector2>();
        playerInputActions.PlayerControls.Jump.performed += context => Jump();
        //playerInputActions.PlayerControls.Attack.performed += context => Attack();
        attack = playerInputActions.PlayerControls.Attack;
        dash = playerInputActions.PlayerControls.Dash;

        state = State.Normal;
    }

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        dashTime = startDashTime;
    }

    void Update()
    {
        Run();
        GroundCheckForJumpAnimation();
        FlipCharacterSprite();
        Dash();
        Attack();
    }

    void Dash()
    {
        if(direction == 0)
        {
            if (dash.triggered)
            {
                Debug.Log("dash");
                if(movementInput.x > 0)
                {
                    direction = 2;
                }
                else if(movementInput.x < 0)
                {
                    direction = 3;
                }
                else if (movementInput.x == 0)
                {
                    direction = transform.localScale.x;
                } 
            }
        } else {
            if(dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rigidBody2D.velocity = Vector2.zero;
            } 
            else 
            {
                dashTime -= Time.deltaTime;

                if (direction == 2)
                {
                    rigidBody2D.velocity = Vector2.right * dashSpeed;
                } 
                else if (direction == 3)
                {
                    rigidBody2D.velocity = Vector2.left * dashSpeed;
                }
                else if (direction == transform.localScale.x)
                {
                    rigidBody2D.velocity = new Vector2(transform.localScale.x * dashSpeed, rigidBody2D.velocity.y);
                }
            }
        }
    }

    void Attack()
    {
        if (attack.triggered)
        {
            Debug.Log("attack");
            if (Time.time >= nextAttackTime)
            {
                state = State.Attacking;
                animator.SetTrigger("Attack");
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                foreach (var enemy in hitEnemies)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                }
                nextAttackTime = Time.time + 1f / attackRate;
                //sound effect 
                //vfx
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint == null) { return; }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void Jump()
    {
        bool isGrounded = myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        
        if(!isGrounded){ return; }

        float yVelocity = jumpSpeed;
        Vector2 jumpVelocity = new Vector2(0, yVelocity);
        //rigidBody2D.AddForce(new Vector2(0, yVelocity));
        rigidBody2D.velocity += jumpVelocity;
    }

    void GroundCheckForJumpAnimation(){
        if (!myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }
    }

    void Run()
    {
        //float controlThrow = Input.GetAxis("Horizontal");
        float controlThrow = movementInput.x;
        float xVelocity = controlThrow * runSpeed;

        Vector2 playerVelocity = new Vector2(xVelocity, rigidBody2D.velocity.y);
        rigidBody2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody2D.velocity.x) > Mathf.Epsilon;
        animator.SetBool("Running", playerHasHorizontalSpeed); 
    }

    void FlipCharacterSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody2D.velocity.x), transform.localScale.y);
        }
    }

    private void OnEnable() {
        playerInputActions.PlayerControls.Enable();
    }

    private void OnDisable() {
        playerInputActions.PlayerControls.Disable();
    }
}

