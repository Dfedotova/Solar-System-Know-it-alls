using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField] private Transform[] pictures;
    
    [SerializeField] private int size;

    public static bool Win;

    private void Start()
    {
        Win = false;
    }

    private void Update()
    {
        if (CheckRotation())
        {
            Win = true;
            Debug.Log("Game over!"); // TODO
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