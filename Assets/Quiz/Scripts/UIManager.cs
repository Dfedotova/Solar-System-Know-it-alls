using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UIManagerParameters
{
    [Header("Answers Options")] [SerializeField]
    private float margins;

    public float Margins => margins;
}

[System.Serializable]
public struct UIElements
{
    [SerializeField] private RectTransform answersContentArea;
    public RectTransform AnswersContentArea => answersContentArea;
    
    [SerializeField] private Text questionInfoTextObject;
    public Text QuestionInfoTextObject => questionInfoTextObject;
}

public class UIManager : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Events events;

    [Header("UI Elements (Prefabs)")] [SerializeField]
    private AnswerData answerPrefab;

    [SerializeField] private UIElements uiElements;

    [Space]
    [SerializeField] private UIManagerParameters parameters;

    private List<AnswerData> _currentAnswers = new List<AnswerData>();

    private void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
    }

    private void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
    }

    private void UpdateQuestionUI(Question question)
    {
        uiElements.QuestionInfoTextObject.text = question.Info;
        CreateAnswers(question);
    }

    private void CreateAnswers(Question question)
    {
        EraseAnswers();

        float offset = 0/* - parameters.Margins*/;
        for (int i = 0; i < question.Answers.Length; i++)
        {
            AnswerData newAnswer = (AnswerData) Instantiate(answerPrefab, uiElements.AnswersContentArea);
            newAnswer.UpdateData(question.Answers[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= newAnswer.Rect.sizeDelta.y + parameters.Margins;
            uiElements.AnswersContentArea.sizeDelta 
                = new Vector2(uiElements.AnswersContentArea.sizeDelta.x, offset * (-1) / 3);
            
            _currentAnswers.Add(newAnswer);
        }
    }

    private void EraseAnswers()
    {
        foreach (var answer in _currentAnswers)
            Destroy(answer.gameObject);
        
        _currentAnswers.Clear();
    }
}