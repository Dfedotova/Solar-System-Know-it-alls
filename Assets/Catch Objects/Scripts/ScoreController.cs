using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public Text scoreText;
    public Text levelScoreText;
    public Text generalScoreText;
    public GameObject infoPanel;
    public GameObject exitPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public Spawner spawner;
    
    private int score;

    void Update()
    {
        scoreText.text = score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if ((infoPanel.activeSelf 
             || exitPanel.activeSelf 
             || pausePanel.activeSelf 
             || gameOverPanel.activeSelf)
            && target.tag == "BlackHole")
            Destroy(target.gameObject);
        else if (PanelsNonActive() && target.tag == "BlackHole")
        {
            spawner.ActivatePanel(gameOverPanel);
            levelScoreText.text = "Ты набрал " + score + " очков!";
            /*counter.(counter.GeneralScore + score);
            generalScoreText.text = counter.GeneralScore.ToString();*/
        }
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Planet")
        {
            if (PanelsNonActive())
                score += 20;
            Destroy(target.gameObject);
        }
        else if (target.tag == "UFO")
        {
            if (PanelsNonActive())
                score += 40;
            Destroy(target.gameObject);
        }
    }

    private bool PanelsNonActive() => !infoPanel.activeSelf
                                      && !exitPanel.activeSelf
                                      && !pausePanel.activeSelf
                                      && !gameOverPanel.activeSelf;
}