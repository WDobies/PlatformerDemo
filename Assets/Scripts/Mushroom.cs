using UnityEngine;

public class Mushroom : Enemy
{
    [SerializeField] private int pushForce = 400;
    [SerializeField] private float stunDuration = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //attack player on collision
        if (collision.CompareTag("Player"))
            if (player)
                Attack();
    }

    private void Attack()
    {
        PushPlayer();
        player.TakeDamage(damage);
    }

    private void PushPlayer()
    {
        player.Stun(stunDuration);
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        // create opposite vector to player velocity + mushroom rotation
        Vector2 pushVector = (player.transform.position - transform.position).normalized;
        Vector2 opposite = pushVector * pushForce;

        playerRb.AddForce(opposite, ForceMode2D.Force);
    }
}
