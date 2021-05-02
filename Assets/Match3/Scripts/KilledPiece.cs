using UnityEngine;
using UnityEngine.UI;

public class KilledPiece : MonoBehaviour
{
    public bool falling;

    private const float Speed = 16f;
    private const float Gravity = 32f;
    private Vector2 _moveDir;
    private RectTransform _rect;
    private Image _img;

    public void Initialize(Sprite piece, Vector2 start)
    {
        falling = true;

        _moveDir = Vector2.up;
        _moveDir.x = Random.Range(-1.0f, 1.0f);
        _moveDir *= Speed / 2;

        _img = GetComponent<Image>();
        _rect = GetComponent<RectTransform>();
        _img.sprite = piece;
        _rect.anchoredPosition = start;
    }

    void Update()
    {
        if (!falling) return;
        _moveDir.y -= Time.deltaTime * Gravity;
        _moveDir.x = Mathf.Lerp(_moveDir.x, 0, Time.deltaTime);
        _rect.anchoredPosition += _moveDir * (Time.deltaTime * Speed);
        if (_rect.position.x < -32f
            || _rect.position.x > Screen.width + 32f
            || _rect.position.y < -32f
            || _rect.position.y > Screen.height + 32f)
            falling = false;
    }
}