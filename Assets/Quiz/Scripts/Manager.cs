using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private Question[] questions = null;
    public Question[] Questions => questions;

    [SerializeField] private Events events = null;
    [SerializeField] private Text scoreText;
    [SerializeField] private int level;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private GameObject pausePanel;

    private List<AnswerData> _pickedAnswers = new List<AnswerData>();
    private List<int> _finishedQuestions = new List<int>();
    private int currentQuestion = 0;
    private int score = 0;

    private IEnumerator IE_WaitTillNextRound = null;

    private bool IsFinished => !(_finishedQuestions.Count < Questions.Length);

    void Start()
    {
        infoPanel.SetActive(false);
        exitPanel.SetActive(false);
        pausePanel.SetActive(false);
        
        LoadQuestions();

        var seed = Random.Range(Int32.MinValue, Int32.MaxValue);
        Random.InitState(seed);

        Display();
    }

    private void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }

    private void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    public void UpdateAnswers(AnswerData newAnswer)
    {
        if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Single)
        {
            foreach (var answer in _pickedAnswers)
                if (answer != newAnswer)
                    answer.Reset();
            _pickedAnswers.Clear();
            _pickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = _pickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
                _pickedAnswers.Remove(newAnswer);
            else
                _pickedAnswers.Add(newAnswer);
        }
    }

    public void EraseAnswers()
    {
        _pickedAnswers = new List<AnswerData>();
    }

    private void Display()
    {
        EraseAnswers();
        var question = GetRandomQuestion();

        events.UpdateQuestionUI?.Invoke(question);
    }

    public void Accept()
    {
        bool isCorrect = CheckAnswers();
        _finishedQuestions.Add(currentQuestion);

        score += isCorrect ? 20 : score > 0 ? -10 : 0;
        scoreText.text = score.ToString();

        // TODO
        if (IsFinished)
        {
            Debug.Log("Game over!");
        }

        if (IE_WaitTillNextRound != null)
            StopCoroutine(IE_WaitTillNextRound);
        IE_WaitTillNextRound = WaitTillNextRound();
        StartCoroutine(IE_WaitTillNextRound);
    }

    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(0.5f);
        Display();
    }

    private Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;

        return Questions[currentQuestion];
    }

    private int GetRandomQuestionIndex()
    {
        var random = 0;
        if (_finishedQuestions.Count < Questions.Length)
        {
            do
            {
                random = Random.Range(0, Questions.Length);
            } while (_finishedQuestions.Contains(random) || random == currentQuestion);
        }

        return random;
    }

    private void LoadQuestions()
    {
        Object[] objects = null;
        if (level == 1)
            objects = Resources.LoadAll("Questions lvl 1", typeof(Question));
        if (level == 2)
            objects = Resources.LoadAll("Questions lvl 2", typeof(Question));
        if (level == 3)
            objects = Resources.LoadAll("Questions lvl 3", typeof(Question));
         
        questions = new Question[objects.Length];
        for (int i = 0; i < objects.Length; i++)
            questions[i] = (Question) objects[i];
    }

    private bool CheckAnswers() => CompareAnswers();

    private bool CompareAnswers()
    {
        if (_pickedAnswers.Count > 0)
        {
            List<int> c = Questions[currentQuestion].GetCorrectAnswers();
            List<int> p = _pickedAnswers.Select(x => x.AnswerIndex).ToList();

            return c.Count == p.Count && !c.Except(p).Any();
        }

        return false;
    }
}