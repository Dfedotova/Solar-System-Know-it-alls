using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManagment : MonoBehaviour
{
    public Sprite[] cardFaces;
    public Sprite cardBack;
    public GameObject[] cards;
    public Text matchText;

    [SerializeField] private int matches;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject exitPanel;

    private bool init = false;

    void Start()
    {
        infoPanel.SetActive(false);
        exitPanel.SetActive(false);
    }

    void Update()
    {
        if (!init)
            InitializeCards();

        if (Input.GetMouseButtonUp(0))
            CheckCards();
    }

    private void InitializeCards()
    {
        for (int id = 0; id < 2; id++)
        {
            for (int i = 1; i < matches + 1; i++)
            {
                bool test = false;
                int choice = 0;

                while (!test)
                {
                    choice = Random.Range(0, cards.Length);
                    test = !cards[choice].GetComponent<CardManager>().Initialized;
                }

                cards[choice].GetComponent<CardManager>().CardValue = i;
                cards[choice].GetComponent<CardManager>().Initialized = true;
            }
        }

        foreach (GameObject c in cards)
            c.GetComponent<CardManager>().SetUpGraphics();

        if (!init)
            init = true;
    }

    public Sprite GetCardBack() => cardBack;

    public Sprite GetCardFace(int i) => cardFaces[i - 1];

    private void CheckCards()
    {
        List<int> c = new List<int>();

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].GetComponent<CardManager>().State == 1)
                c.Add(i);
        }

        if (c.Count == 2)
            CardComparison(c);
    }

    private void CardComparison(List<int> c)
    {
        CardManager.DoNot = true;

        int x = 0;

        if (cards[c[0]].GetComponent<CardManager>().CardValue == cards[c[1]].GetComponent<CardManager>().CardValue)
        {
            x = 2;
            matches--;
            matchText.text = matches.ToString();
            if (matches == 0)
                SceneManager.LoadScene("Levels Pair Matching"); // TODO поменять потом на сцену "Конец игры"
        }

        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<CardManager>().State = x;
            cards[c[i]].GetComponent<CardManager>().FalseCheck();
        }
    }
}