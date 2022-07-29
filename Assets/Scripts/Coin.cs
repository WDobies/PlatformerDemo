using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    public static Action<int> OnCoinCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnCoinCollected?.Invoke(coinValue);
    }
}