using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Action<int> OnHealthChange;

    [Header("Player stats")]
    [Range(1, 3)]
    [SerializeField] private int healthPoints = 3;
    [SerializeField] private int damage = 1;

    [Header("Movement")]
    [SerializeField] private int movementSpeed = 5;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float fallMultiplayer = 2;
    [SerializeField] private float jumpCollisionRadius = 0.15f;
    [SerializeField] private Vector2 jumpOffset = new Vector2(0, -0.5f);
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Attack")]
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask enemiesLayerMask;

    private bool canMove = true;
    private Rigidbody2D playerRigidbody;
    private Animator animator;

    private const float MIN_X_INPUT = 0.1f;
    private const float MIN_Y_VELOCITY = -0.1f;

    private void Awake()
    {
        //load components
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //set health on UI
        OnHealthChange?.Invoke(healthPoints);
    }
    private void Update()
    {
        Falling();
    }

    #region Movement
    public void Movement(Vector2 input)
    {
        //player roataion
        if (input.x < -MIN_X_INPUT)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (input.x > MIN_X_INPUT)
        {
            transform.rotation = Quaternion.identity;
        }
        //set speed for Run animation
        animator.SetFloat("speed", Mathf.Abs(input.x));

        if (canMove)
            playerRigidbody.velocity = new Vector2(input.x * movementSpeed, playerRigidbody.velocity.y);
    }
    public void Stun(float duration)
    {
        //block player movement and remove effect after duration
        canMove = false;
        Invoke("RemoveStun", duration);
    }
    public void RemoveStun()
    {
        canMove = true;
    }
    public void Jump()
    {
        //Perform jump only if player is touching groundLayer
        if (isGrounded() && canMove)
        {
            animator.SetTrigger("Jump");
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
        }
    }
    public bool isGrounded()
    {
        return Physics2D.OverlapCircle((Vector2)transform.position + jumpOffset, jumpCollisionRadius, groundLayerMask);
    }
    private void Falling()
    {
        //increase gravity when player is falling
        if (playerRigidbody.velocity.y < MIN_Y_VELOCITY)
        {
            playerRigidbody.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplayer * Time.deltaTime;
            animator.SetBool("IsFalling", true);
        }
        else
        {
            animator.SetBool("IsFalling", false);
        }
    }
    #endregion

    #region Attack
    public void PerformAttack()
    {
        //start attack animation
        if (canMove)
            animator.SetTrigger("Attack");
    }

    //function called in second frame of attack animation 
    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackTransform.position, attackRadius, enemiesLayerMask);

        foreach (var item in enemies)
        {
            Enemy enemy = item.GetComponent<Enemy>();
            if (enemy)
                enemy.TakeDamage(damage);
        }
    }
    #endregion

    #region TakeDamage
    public void TakeDamage(int dmg)
    {
        //remove health points
        healthPoints -= dmg;
        animator.SetTrigger("Hit");
        //remove healt points on UI
        OnHealthChange?.Invoke(healthPoints);
        if (healthPoints <= 0)
        {
            animator.SetBool("IsDead", true);
        }
    }

    //method called in last frame of death animation
    private void Die()
    {
        Destroy(gameObject);
    }
    #endregion

    #region DEBUG
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackTransform.position, attackRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + jumpOffset, jumpCollisionRadius);
    }
    #endregion
}
