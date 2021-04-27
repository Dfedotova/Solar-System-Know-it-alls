using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Result : MonoBehaviour
{
    public Questions questions;
    public Button trueButton;
    public Button falseButton;
    public Scores scores;
    public UnityEvent onNextQuestion;

    public void ShowResults(bool answer)
    {
        if (questions.questionsList[questions.currentQuestion].isTrue == answer)
            scores.UpdateScore(20);
        else
            scores.UpdateScore(-10);

        trueButton.interactable = false;
        falseButton.interactable = false;

        StartCoroutine(ShowResult());
    }

    private IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(0.5f);

        trueButton.interactable = true;
        falseButton.interactable = true;

        onNextQuestion.Invoke();
    }
}