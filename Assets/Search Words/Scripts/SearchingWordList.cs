using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWordList : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Text scoreText;
    public Text levelScoreText;
    public GameData currentGameData;
    public GameObject searchingWordPrefab;
    public float offset = 0.0f;
    public int maxColumns = 5;
    public int maxRows = 4;

    private int _columns = 2;
    private int _rows;
    private int _wordsNumber;

    private List<GameObject> _words = new List<GameObject>();

    void Start()
    {
        _wordsNumber = currentGameData.selectedBoardData.searchWords.Count;

        if (_wordsNumber < _columns)
            _rows = 1;
        else
            CalculateColumnsAndRowsNumber();

        CreateWordObjects();
        SetWordsPosition();
    }

    private void Update()
    {
        if (int.Parse(scoreText.text) / 40 == _wordsNumber)
        {
            gameOverPanel.SetActive(true);
            levelScoreText.text = "Ты набрал " + scoreText.text + " очков!";
        }
    }

    private void CalculateColumnsAndRowsNumber()
    {
        do
        {
            _columns++;
            _rows = _wordsNumber / _columns;
        } while (_rows >= maxRows);

        if (_columns > maxColumns)
        {
            _columns = maxColumns;
            _rows = _wordsNumber / _columns;
        }
    }

    private bool TryIncreaseColumnNumber()
    {
        _columns++;
        _rows = _wordsNumber / _columns;

        if (_columns > maxColumns)
        {
            _columns = maxColumns;
            _rows = _wordsNumber / _columns;

            return false;
        }

        if (_wordsNumber % _columns > 0)
            _rows++;

        return true;
    }

    private void CreateWordObjects()
    {
        var squareScale = GetSquareScale(new Vector3(1f, 1f, 0.1f));

        for (int i = 0; i < _wordsNumber; i++)
        {
            _words.Add(Instantiate(searchingWordPrefab) as GameObject);
            _words[i].transform.SetParent(this.transform);
            _words[i].GetComponent<RectTransform>().localScale = squareScale;
            _words[i].GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
            _words[i].GetComponent<SearchingWord>().SetWord(currentGameData.selectedBoardData.searchWords[i].word);
        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;

        var adjustment = 4.5f;

        while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;

            if (finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;
                return finalScale;
            }
        }

        return finalScale / 1.2f;
    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = searchingWordPrefab.GetComponent<RectTransform>();
        var parentRect = this.GetComponent<RectTransform>();

        var squareSize = new Vector2(0f, 0f);
        squareSize.x = squareRect.rect.width * targetScale.x + offset;
        squareSize.y = squareRect.rect.height * targetScale.y + offset;

        var totalSquareHeights = squareSize.y * _rows;

        if (totalSquareHeights > parentRect.rect.height)
        {
            while (totalSquareHeights > parentRect.rect.height)
            {
                if (TryIncreaseColumnNumber())
                    totalSquareHeights = squareSize.y * _rows;
                else
                    return true;
            }
        }

        var totalSquareWidth = squareSize.x * _columns;

        return totalSquareWidth > parentRect.rect.width;
    }

    private void SetWordsPosition()
    {
        var squareRect = _words[0].GetComponent<RectTransform>();
        var wordOffset = new Vector2
        {
            x = squareRect.rect.width * squareRect.transform.localScale.x + offset,
            y = squareRect.rect.height * squareRect.transform.localScale.y + offset
        };

        int columnNumber = 0;
        int rowNumber = 0;
        var startPosition = GetFirstSquarePosition();

        foreach (var word in _words)
        {
            if (columnNumber + 1 > _columns)
            {
                columnNumber = 0;
                rowNumber++;
            }

            var positionX = startPosition.x + wordOffset.x * columnNumber;
            var positionY = (startPosition.y + wordOffset.y * rowNumber) / 1.5f;

            word.GetComponent<RectTransform>().localPosition = new Vector2(positionX, positionY);
            columnNumber++;
        }
    }

    private Vector2 GetFirstSquarePosition()
    {
        var startPosition = new Vector2(0f, transform.position.y);
        var squareRect = _words[0].GetComponent<RectTransform>();
        var parentRect = this.GetComponent<RectTransform>();
        var squareSize = new Vector2(0f, 0f);

        squareSize.x = squareRect.rect.width * squareRect.transform.localScale.x + offset; 
        squareSize.y = squareRect.rect.height * squareRect.transform.localScale.y + offset;

        var shiftBy = (parentRect.rect.width - (squareSize.x * _columns)) / 2;

        startPosition.x = ((parentRect.rect.width - squareSize.x) / 2) * (-1);
        startPosition.x += shiftBy;
        startPosition.y = (parentRect.rect.height - squareSize.y) / 2;

        return startPosition / 30; 
    }
}