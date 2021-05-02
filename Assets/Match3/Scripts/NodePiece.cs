using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int value;
    public Point index;

    [HideInInspector] public Vector2 position;

    [HideInInspector] public RectTransform rect;

    private bool _updating;
    private Image _img;

    public void Initialize(int v, Point point, Sprite piece)
    {
        _img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        value = v;
        SetIndex(point);
        _img.sprite = piece;
    }

    public void SetIndex(Point point)
    {
        index = point;
        ResetPosition();
        UpdateName();
    }

    public void ResetPosition()
    {
        position = new Vector2(-258 + (40 * index.x), 158 - (40 * index.y));
    }

    public void MovePosition(Vector2 move)
    {
        rect.anchoredPosition += move * Time.deltaTime * 16f;
    }

    public void MovePositionTo(Vector2 move)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 16f);
    }

    private void UpdateName()
    {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    public bool UpdatePiece()
    {
        if (Vector3.Distance(rect.anchoredPosition, position) > 1)
        {
            MovePositionTo(position);
            _updating = true;
            return true;
        }

        rect.anchoredPosition = position;
        _updating = false;
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_updating) return;
        MovePieces.Instance.MovePiece(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MovePieces.Instance.DropPiece();
    }
}