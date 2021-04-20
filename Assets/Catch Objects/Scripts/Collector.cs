using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Planet" || target.tag == "BlackHole" || target.tag == "UFO")
            Destroy(target.gameObject);
    }
}