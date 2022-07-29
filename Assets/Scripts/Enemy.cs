using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy stats")]
    [SerializeField] protected int damage = 1;
    [SerializeField] protected int healthPoints = 2;
    [SerializeField] protected float movementSpeed = 1.5f;
    [SerializeField] protected List<Transform> patrolPoints;

    protected Player player;
    protected Animator animator;

    protected int currentPoint = 0;
    protected const float MIN_DISTANCE_TO_POINT = 0.4f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (patrolPoints.Count != 0)
        {
            RotateTowards(patrolPoints[currentPoint]);
            animator.SetBool("IsPatroling", true);
        }
    }

    private void FixedUpdate()
    {
        Behaviour();
    }
    virtual protected void Behaviour()
    {
        //patrol as default behavior for all enemies
        Patrol();
    }

    #region Movement
    protected void Patrol()
    {
        if (patrolPoints.Count != 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPoint].position, movementSpeed * Time.deltaTime);

            //target another patrol point after if current point has been reached 
            if (Vector2.Distance(transform.position, patrolPoints[currentPoint].position) < MIN_DISTANCE_TO_POINT)
            {
                currentPoint++;

                //reset to first patroll point
                if (patrolPoints.Count == currentPoint)
                    currentPoint = 0;

                RotateTowards(patrolPoints[currentPoint]);
            }
        }
    }
    protected void RotateTowards(Transform target)
    {
        float direction = transform.position.x - target.position.x;
        transform.right = direction > 0 ? -Vector2.right : Vector2.right;
    }
    #endregion

    #region TakeDamage
    public void TakeDamage(int dmg)
    {
        //remove health points
        healthPoints -= dmg;
        animator.SetTrigger("Hit");
        if (healthPoints <= 0)
        {
            animator.SetBool("IsDead", true);
            //clear patrol points to stop movement
            patrolPoints.Clear();
        }
    }

    //method called in last frame of death animation
    private void Die()
    {
        Destroy(gameObject);
    }
    #endregion
}
