using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    private static int _generalScore;
    public Text finalScore;
    public Text gameScore;

    private void Start()
    {
        int score;
        int.TryParse(string.Join(string.Empty, 
            finalScore.text.Where(c => char.IsDigit(c))), out score);
        _generalScore += score;
        gameScore.text = _generalScore.ToString();
    }
}
