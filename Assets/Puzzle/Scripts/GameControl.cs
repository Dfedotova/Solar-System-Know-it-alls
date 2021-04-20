using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField] private Transform[] pictures;

    [SerializeField] private GameObject winImage;

    [SerializeField] private int size;

    public static bool Win;

    private void Start()
    {
        Win = false;
        winImage.SetActive(false);
    }

    private void Update()
    {
        if (CheckRotation())
        {
            Win = true;
            winImage.SetActive(true);
        }
    }

    private bool CheckRotation()
    {
        for (int i = 0; i < size; i++)
            if (pictures[i].rotation.z != 0)
                return false;
        return true;
    }
}