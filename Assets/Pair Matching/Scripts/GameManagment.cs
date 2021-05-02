using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManagment : MonoBehaviour
{
    public Sprite[] cardFaces;
    public Sprite cardBack;
    public GameObject[] cards;
    public Text matchText;
    public Text levelScoreText;
    public int matches;
    public GameObject infoPanel;
    public GameObject exitPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    private bool _init = false;
    private int _matchesTmp;

    void Start()
    {
        _matchesTmp = matches;
        infoPanel.SetActive(false);
        exitPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (!_init)
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

        if (!_init)
            _init = true;
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

        if (cards[c[0]].GetComponent<CardManager>().CardValue == cards[c[1]].
            GetComponent<CardManager>().CardValue)
        {
            x = 2;
            matches--;
            matchText.text = matches.ToString();
            if (matches == 0)
            {
                gameOverPanel.SetActive(true);
                levelScoreText.text = "Ты набрал " + _matchesTmp * 100 + " очков!";
            }
        }

        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<CardManager>().State = x;
            cards[c[i]].GetComponent<CardManager>().FalseCheck();
        }
    }
}