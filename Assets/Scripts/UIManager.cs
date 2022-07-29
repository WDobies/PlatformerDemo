using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private List<GameObject> healthPoints;

    private int score;

    private void Awake()
    {
        Coin.OnCoinCollected += SetScore;
        Player.OnHealthChange += SetActiveHealthPoints;
    }
    public void SetScore(int value)
    {
        score += value;
        scoreText.SetText(score.ToString());
    }

    public void SetActiveHealthPoints(int points)
    {
        int counter = 0;
        foreach (GameObject hp in healthPoints)
        {
            if (counter < points)
                hp.SetActive(true);
            else
                hp.SetActive(false);

            counter++;
        }
    }
    private void OnDisable()
    {
        Coin.OnCoinCollected -= SetScore;
        Player.OnHealthChange -= SetActiveHealthPoints;
    }
}
