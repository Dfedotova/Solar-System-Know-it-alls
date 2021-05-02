using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
    public Text scoreText;
    private int _currentScore;
    
    void Start()
    {
        _currentScore = 0;
        scoreText.text = _currentScore.ToString();
    }

    public void UpdateScore(int score)
    {
        _currentScore += score > 0 ? score : _currentScore > 0 ? score : 0;
        scoreText.text = _currentScore.ToString();
    }
}
