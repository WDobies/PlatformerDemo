using UnityEngine;

public class Goblin : Enemy
{
    [SerializeField] private float distanceToAttack = 1.2f;
    [SerializeField] private float chaseDistance = 2.0f;
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float attackRadius = 0.45f;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform attackTransform;

    protected override void Behaviour()
    {
        if (player)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance < distanceToAttack)
            {
                animator.SetTrigger("Attack");
            }
            else if (distance < chaseDistance)
            {
                RotateTowards(player.transform);
                animator.SetBool("IsPatroling", true);
                Collider2D groundCollider = Physics2D.OverlapCircle(attackTransform.position, attackRadius, groundLayerMask);

                //check ground collider before next step to awoid falling off the platform
                if (groundCollider)
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                //if has patrol points start patroling, else turn off walking animation
                if (patrolPoints.Count != 0)
                {
                    RotateTowards(patrolPoints[currentPoint]);
                    Patrol();
                }
                else
                    animator.SetBool("IsPatroling", false);
            }
        }
    }

    public void Attack()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(attackTransform.position, attackRadius, playerLayerMask);
        if (playerCollider)
            player.TakeDamage(damage);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackTransform.position, attackRadius);
    }
}
