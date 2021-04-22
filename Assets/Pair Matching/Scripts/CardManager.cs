using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static bool DoNot = false;

    [SerializeField] 
    private int state;

    [SerializeField] 
    private int cardValue;

    [SerializeField] 
    private bool initialized = false;

    private Sprite cardBack;
    private Sprite cardFace;

    private GameObject manager;

    private void Start()
    {
        state = 1;
        manager = GameObject.FindGameObjectWithTag("Manager");
    }

    public void SetUpGraphics()
    {
        cardBack = manager.GetComponent<GameManagment>().GetCardBack();
        cardFace = manager.GetComponent<GameManagment>().GetCardFace(cardValue);

        FlipCard();
    }

    public void FlipCard()
    {
        if (state == 0)
            state = 1;
        else if (state == 1)
            state = 0;
        
        if (state == 0 && !DoNot)
            GetComponent<Image>().sprite = cardBack;
        else if (state == 1 && !DoNot)
            GetComponent<Image>().sprite = cardFace;
    }

    public int CardValue
    {
        get => cardValue;
        set => cardValue = value;
    }

    public int State
    {
        get => state;
        set => state = value;
    }
    
    public bool Initialized
    {
        get => initialized;
        set => initialized = value;
    }

    public void FalseCheck()
    {
        StartCoroutine(Pause());
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(1);
        if (state == 0)
            GetComponent<Image>().sprite = cardBack;
        else if (state == 1)
            GetComponent<Image>().sprite = cardFace;
        DoNot = false;
    }
}
