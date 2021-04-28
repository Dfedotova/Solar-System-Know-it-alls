using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    [SerializeField] private Transform[] pictures;

    [SerializeField] private int size;
    [SerializeField] private Text levelScoreText;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    public static bool Win;

    private void Start()
    {
        Win = false;
        infoPanel.SetActive(false);
        exitPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (CheckRotation())
        {
            Win = true;
            gameOverPanel.SetActive(true);
            levelScoreText.text = "Ты набрал " + size * 100 + " очков!";
        }
    }

    private bool CheckRotation()
    {
        for (int i = 0; i < size; i++)
            if (pictures[i].rotation.z != 0)
                return false;
        return true;
    }
}