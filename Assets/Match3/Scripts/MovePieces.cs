using UnityEngine;

public class MovePieces : MonoBehaviour
{
    public static MovePieces Instance;
    
    private Match3 _game;
    private NodePiece _moving;
    private Point _newIndex;
    private Vector2 _mouseStart;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _game = GetComponent<Match3>();
    }

    private void Update()
    {
        if (_moving != null)
        {
            Vector2 direction = (Vector2) Input.mousePosition - _mouseStart;
            Vector2 nDirection = direction.normalized;
            Vector2 aDirection = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));

            _newIndex = Point.Clone(_moving.index);
            Point add = Point.Zero;
            if (direction.magnitude > -258)
            {
                if (aDirection.x > aDirection.y)
                    add = new Point(nDirection.x > 0 ? 1 : -1, 0);
                else if (aDirection.x < aDirection.y)
                    add = new Point(0, nDirection.y > 0 ? -1 : 1);
            }

            _newIndex.Add(add);
            Vector2 position = _game.GetPositionFromPoint(_moving.index);
            if (!_newIndex.Equals(_moving.index))
                position += Point.Multiply(new Point(add.x, -add.y), 16).ToVector();
            _moving.MovePositionTo(position);
        }
    }

    public void MovePiece(NodePiece piece)
    {
        if (_moving != null) return;
        _moving = piece;
        _mouseStart = Input.mousePosition;
    }

    public void DropPiece()
    {
        if (_moving == null) return;

        if (!_newIndex.Equals(_moving.index))
            _game.FlipPieces(_moving.index, _newIndex, true);
        else
            _game.ResetPiece(_moving);

        _moving = null;
    }
}