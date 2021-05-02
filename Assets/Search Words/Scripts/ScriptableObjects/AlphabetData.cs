using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class AlphabetData : ScriptableObject
{
    public List<LetterData> alphabetPlain = new List<LetterData>();
    public List<LetterData> alphabetNormal = new List<LetterData>();
    public List<LetterData> alphabetHighlighted = new List<LetterData>();
    public List<LetterData> alphabetWrong = new List<LetterData>();
    
    [System.Serializable]
    public class LetterData
    {
        public string letter;
        public Sprite image;
    }
}
