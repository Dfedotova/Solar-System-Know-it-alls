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

        /*if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touchPosition.x > 0)
                _myBody.velocity = new Vector2(speed, _myBody.velocity.y);
            else if (touchPosition.x < 0)
                _myBody.velocity = new Vector2(-speed, _myBody.velocity.y);
        }
        else
            _myBody.velocity = new Vector2(0f, _myBody.velocity.y);*/

        transform.position = new Vector2(Mathf.Clamp(
            transform.position.x, -xBound, xBound), transform.position.y);
    }
}