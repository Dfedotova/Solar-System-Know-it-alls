using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsData : MonoBehaviour
{
    public Questions questions;
    [SerializeField] private Text questionText;

    void Start()
    {
        AskQuestion();
    }

    public void AskQuestion()
    {
        if (CountValidQuestions() == 0)
        {
            questionText.text = string.Empty;
            ClearQuestions();
            Debug.Log("Game over!"); // TODO
            return;
        }

        var randomIndex = 0;
        do
        {
            randomIndex = Random.Range(0, questions.questionsList.Count);
        } while (questions.questionsList[randomIndex].questioned);

        questions.currentQuestion = randomIndex;
        questions.questionsList[questions.currentQuestion].questioned = true;
        questionText.text = questions.questionsList[questions.currentQuestion].question;
    }
    
    private void ClearQuestions()
    {
        foreach (var question in questions.questionsList)
            question.questioned = false;
    }

    private int CountValidQuestions()
    {
        int validQuestions = 0;

        foreach (var question in questions.questionsList)
        {
            if (!question.questioned)
                validQuestions++;
        }

        return validQuestions;
    }
}