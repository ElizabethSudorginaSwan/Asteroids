using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [field: SerializeField] public TMP_Text ScoreText { get; private set; }

    private int totalScore;

    private void Start()
    {
        totalScore = 0;
    }
    public void AddScore(int points) 
    {
        totalScore += points;
        ScoreText.text = $"{totalScore}";
        Debug.Log("Total Score: " + totalScore);
    }

    public void ResetScore() 
    {
        totalScore = 0;
        ScoreText.text = $"{totalScore}";
    }
}
