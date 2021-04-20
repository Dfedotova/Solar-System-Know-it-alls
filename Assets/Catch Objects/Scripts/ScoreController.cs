using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    public Text scoreText;

    private int score;

    void Update()
    {
        scoreText.text = score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "BlackHole")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // todo загружаем сцену "Вы проиграли"
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Planet")
        {
            Destroy(target.gameObject);
            score += 20;
        }
        else if (target.tag == "UFO")
        {
            Destroy(target.gameObject);
            score += 40;
        }
    }
}