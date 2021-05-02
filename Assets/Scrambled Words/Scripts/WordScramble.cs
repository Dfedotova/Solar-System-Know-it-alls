using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class Word
{
    public string word;

    public string GetString()
    {
        string result = word;

        while (result == word)
        {
            result = string.Empty;

            List<char> characters = new List<char>(word.ToCharArray());
            while (characters.Count > 0)
            {
                int index = Random.Range(0, characters.Count - 1);
                result += characters[index];

                characters.RemoveAt(index);
            }
        }

        return result;
    }
}

public class WordScramble : MonoBehaviour
{
    public Word[] words;

    [Header("UI reference")] public CharObject prefab;
    public Transform container;
    public float space;
    public float lerpSpeed = 5;
    public Text scoresText;
    public Text levelScoreText;
    public GameObject infoPanel;
    public GameObject exitPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    private List<CharObject> _charObjects = new List<CharObject>();
    private CharObject _firstSelected;
    private int _scores;

    public int currentWord;

    public static WordScramble Main;

    private void Awake()
    {
        Main = this;
    }

    void Start()
    {
        infoPanel.SetActive(false);
        exitPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        ShowScramble(currentWord);
    }

    void Update()
    {
        RepositionObject();
        scoresText.text = _scores.ToString();
    }

    private void RepositionObject()
    {
        if (_charObjects.Count == 0)
            return;

        float center = (_charObjects.Count - 1) / 2;
        for (int i = 0; i < _charObjects.Count; i++)
        {
            _charObjects[i].rectTransform.anchoredPosition =
                Vector2.Lerp(_charObjects[i].rectTransform.anchoredPosition,
                    new Vector2((i - center) * space, 0), lerpSpeed * Time.deltaTime);
            _charObjects[i].index = i;
        }
    }

    public void ShowScramble()
    {
        ShowScramble(Random.Range(0, words.Length - 1));
    }

    public void ShowScramble(int index)
    {
        _charObjects.Clear();
        foreach (Transform child in container)
            Destroy(child.gameObject);


        if (index > words.Length - 1)
        {
            gameOverPanel.SetActive(true);
            levelScoreText.text = "Ты набрал " + _scores + " очков!";
            return;
        }

        char[] chars = words[index].GetString().ToCharArray();
        foreach (char c in chars)
        {
            CharObject clone = Instantiate(prefab.gameObject).GetComponent<CharObject>();
            clone.transform.SetParent(container);
            clone.rectTransform.localScale = new Vector3(1f, 1f, 1f);

            _charObjects.Add(clone.Init(c));
        }

        currentWord = index;
    }

    public void Swap(int indexA, int indexB)
    {
        CharObject tmpA = _charObjects[indexA];
        _charObjects[indexA] = _charObjects[indexB];
        _charObjects[indexB] = tmpA;

        _charObjects[indexA].transform.SetAsLastSibling();
        _charObjects[indexB].transform.SetAsLastSibling();

        CheckWord();
    }

    public void Select(CharObject charObject)
    {
        if (_firstSelected)
        {
            Swap(_firstSelected.index, charObject.index);
            _firstSelected.Select();
            charObject.Select();
        }
        else
            _firstSelected = charObject;
    }

    public void UnSelect()
    {
        _firstSelected = null;
    }

    public void CheckWord()
    {
        StartCoroutine(CoCheckWord());
    }

    IEnumerator CoCheckWord()
    {
        yield return new WaitForSeconds(0.5f);

        string word = string.Empty;
        foreach (CharObject charObject in _charObjects)
            word += charObject.character;

        if (word == words[currentWord].word)
        {
            _scores += 20;
            currentWord++;
            ShowScramble(currentWord);
        }
    }
}