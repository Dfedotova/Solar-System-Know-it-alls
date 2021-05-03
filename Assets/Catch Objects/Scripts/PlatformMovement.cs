using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private Rigidbody2D _myBody;

    public float speed;
    public float xBound;
    public Joystick joystick;

    private float _horizontalMove;

    void Start()
    {
        _myBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _horizontalMove = joystick.Horizontal * speed;
        if (_horizontalMove > 0)
            _myBody.velocity = Vector2.right * speed;
        else if (_horizontalMove < 0)
            _myBody.velocity = Vector2.left * speed;
        else
            _myBody.velocity = Vector2.zero;

        transform.position = new Vector2(Mathf.Clamp(
            transform.position.x, -xBound, xBound), transform.position.y);
    }
}