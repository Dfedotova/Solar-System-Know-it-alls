using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    public Text scoreText;
    public GameObject infoPanel;
    public GameObject exitPanel;
    public GameObject pausePanel;

    private int score;

    void Update()
    {
        scoreText.text = score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if ((infoPanel.activeSelf || exitPanel.activeSelf || pausePanel.activeSelf) 
            && target.tag == "BlackHole")
            Destroy(target.gameObject);
        else if (!infoPanel.activeSelf && !exitPanel.activeSelf && !pausePanel.activeSelf 
                 && target.tag == "BlackHole")
            Debug.Log("Game over!"); // TODO
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Planet")
        {
            if (!infoPanel.activeSelf && !exitPanel.activeSelf && !pausePanel.activeSelf)
                score += 20;
            Destroy(target.gameObject);
        }
        else if (target.tag == "UFO")
        {
            if (!infoPanel.activeSelf && !exitPanel.activeSelf && !pausePanel.activeSelf)
                score += 40;
            Destroy(target.gameObject);
        }
    }
}