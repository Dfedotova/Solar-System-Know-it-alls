using UnityEngine;

[CreateAssetMenu(fileName = "Events", menuName = "Quiz/new Events")]
public class Events : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Question question);
    public UpdateQuestionUICallback UpdateQuestionUI;
    
    public delegate void UpdateQuestionAnswerCallback(AnswerData pickedAnswer);
    public UpdateQuestionAnswerCallback UpdateQuestionAnswer;
}
