using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharObject : MonoBehaviour
{
    public char character;
    public Text text;
    public Image image;
    public RectTransform rectTransform;
    public int index;

    private bool _isSelected = false;

    public CharObject Init(char c)
    {
        character = c;
        text.text = c.ToString();
        gameObject.SetActive(true);
        return this;
    }

    public void Select()
    {
        _isSelected = !_isSelected;

        if (_isSelected)
            WordScramble.Main.Select(this);
        else WordScramble.Main.UnSelect();
    }
}
