using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerData : MonoBehaviour
{
    [Header("UI Elements")] [SerializeField]
    private Text infoTextObject;

    [SerializeField] private Image toggle;

    [Header("Textures")] [SerializeField] private Sprite uncheckedToggle;
    [SerializeField] private Sprite checkedToggle;

    [Header("References")] [SerializeField]
    private Events events;

    private int answerIndex = -1;
    public int AnswerIndex => answerIndex;

    private RectTransform rect;

    public RectTransform Rect
    {
        get {
            if (rect == null)
                rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            return rect;
        }
    }

    private bool Checked = false;

    public void UpdateData(string info, int index)
    {
        infoTextObject.text = info;
        answerIndex = index;
    }

    public void Reset()
    {
        Checked = false;
        UpdateUI();
    }

    public void SwitchState()
    {
        Checked = !Checked;
        UpdateUI();

        events.UpdateQuestionAnswer?.Invoke(this);
    }

    private void UpdateUI()
    {
        if (toggle == null) return;
        toggle.sprite = Checked ? checkedToggle : uncheckedToggle;
    }
}