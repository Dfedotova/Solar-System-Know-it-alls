using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public static int GeneralScore = 0;
    public Text finalScore;
    public Text gameScore;

    private void Start()
    {
        int score;
        int.TryParse(string.Join("", 
            finalScore.text.Where(c => char.IsDigit(c))), out score);
        GeneralScore += score;
        gameScore.text = GeneralScore.ToString();
    }
}
