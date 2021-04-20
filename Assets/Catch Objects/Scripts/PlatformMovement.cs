using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private Rigidbody2D _myBody;

    public float speed;
    public float xBound;

    void Start()
    {
        _myBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (h > 0)
            _myBody.velocity = Vector2.right * speed;
        else if (h < 0)
            _myBody.velocity = Vector2.left * speed;
        else
            _myBody.velocity = Vector2.zero;

        transform.position = new Vector2(Mathf.Clamp(
            transform.position.x, -xBound, xBound), transform.position.y);
    }
}