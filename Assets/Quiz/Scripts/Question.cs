using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Answer
{
    [SerializeField] private string info;
    public string Info => info;

    [SerializeField] private bool isCorrect;
    public bool IsCorrect => isCorrect;
}

[CreateAssetMenu(fileName = "Question", menuName = "Quiz/new Question")]
public class Question : ScriptableObject
{
    public enum AnswerType
    {
        Multi,
        Single
    }

    [SerializeField] private string info = string.Empty;
    public string Info => info;

    [SerializeField] private Answer[] answers = null;
    public Answer[] Answers => answers;

    [SerializeField] private AnswerType answerType = AnswerType.Multi;
    public AnswerType GetAnswerType => answerType;

    public List<int> GetCorrectAnswers()
    {
        List<int> correctAnswers = new List<int>();
        for (int i = 0; i < Answers.Length; i++)
        {
            if (Answers[i].IsCorrect)
                correctAnswers.Add(i);
        }

        return correctAnswers;
    }
}