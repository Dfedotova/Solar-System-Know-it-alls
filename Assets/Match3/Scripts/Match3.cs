using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Random = System.Random;

public class Match3 : MonoBehaviour
{
    public ArrayLayout boardLayout;

    [Header("UI Elements")] public Sprite[] pieces;
    public RectTransform gameBoard;
    public RectTransform killedBoard;

    [Header("Prefabs")] public GameObject nodePiece;
    public GameObject killedPiece;

    private const int Width = 14;
    private const int Height = 9;
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
    private int[] _fills;
    private Node[,] _board;
    private List<NodePiece> _update;
    private List<FlippedPieces> _flipped;
    private List<NodePiece> _dead;
    private List<KilledPiece> _killed;

    private Random _random;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        _fills = new int[Width];
        string seed = GetSeed();
        _random = new Random(seed.GetHashCode());
        _update = new List<NodePiece>();
        _flipped = new List<FlippedPieces>();
        _dead = new List<NodePiece>();
        _killed = new List<KilledPiece>();

        InitializeBoard();
        VerifyBoard();
        InstantiateBoard();
    }

    private void Update()
    {
        List<NodePiece> finishedUpdating = new List<NodePiece>();
        for (int i = 0; i < _update.Count; i++)
        {
            NodePiece piece = _update[i];
            if (!piece.UpdatePiece())
                finishedUpdating.Add(piece);
        }

        for (int i = 0; i < finishedUpdating.Count; i++)
        {
            NodePiece piece = finishedUpdating[i];
            FlippedPieces flip = GetFlipped(piece);
            NodePiece flippedPiece = null;

            int x = piece.index.x;
            _fills[x] = Mathf.Clamp(_fills[x] - 1, 0, Width);

            List<Point> connected = IsConnected(piece.index, true);

            bool wasFlipped = flip != null;
            if (wasFlipped)
            {
                flippedPiece = flip.GetOtherPiece(piece);
                AddPoints(ref connected, IsConnected(flippedPiece.index, true));
            }

            if (connected.Count == 0)
            {
                if (wasFlipped)
                    FlipPieces(piece.index, flippedPiece.index, false);
            }
            else
            {
                foreach (Point p in connected)
                {
                    KillPiece(p);
                    Node node = GetNodeAtPoint(p);
                    NodePiece nodePiece = node.GetPiece();
                    if (nodePiece != null)
                    {
                        nodePiece.gameObject.SetActive(false);
                        _dead.Add(nodePiece);
                    }

                    node.SetPiece(null);
                }

                ApplyGravityToBoard();
            }

            _flipped.Remove(flip);
            _update.Remove(piece);
        }
    }

    private FlippedPieces GetFlipped(NodePiece p)
    {
        FlippedPieces flip = null;
        for (int i = 0; i < _flipped.Count; i++)
        {
            if (_flipped[i].GetOtherPiece(p) != null)
            {
                flip = _flipped[i];
                break;
            }
        }

        return flip;
    }

    private string GetSeed()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 20; i++)
            sb.Append(Chars[UnityEngine.Random.Range(0, Chars.Length)]);

        return sb.ToString();
    }

    private void InstantiateBoard()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Node node = GetNodeAtPoint(new Point(x, y));
                int value = node.value;
                if (value <= 0) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                NodePiece piece = p.GetComponent<NodePiece>();
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(-258 + (40 * x), 158 - (40 * y));
                piece.Initialize(value, new Point(x, y), pieces[value - 1]);
                node.SetPiece(piece);
            }
        }
    }

    private void InitializeBoard()
    {
        _board = new Node[Width, Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
                _board[x, y] = new Node(boardLayout.rows[y].row[x] ? -1 : FillPiece(), new Point(x, y));
        }
    }

    private int FillPiece()
    {
        int value = 1;
        value = _random.Next(0, 100) / (100 / pieces.Length) + 1;
        return value;
    }

    private void VerifyBoard()
    {
        List<int> remove;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Point point = new Point(x, y);
                int value = GetValueAtPoint(point);
                if (value <= 0) continue;

                remove = new List<int>();
                while (IsConnected(point, true).Count > 0)
                {
                    value = GetValueAtPoint(point);
                    if (!remove.Contains(value))
                        remove.Add(value);
                    SetValueToPoint(point, NewValue(ref remove));
                }
            }
        }
    }

    private int NewValue(ref List<int> remove)
    {
        List<int> available = new List<int>();
        for (int i = 0; i < pieces.Length; i++)
            available.Add(i + 1);

        foreach (int i in remove)
            available.Remove(i);

        if (available.Count <= 0)
            return 0;
        return available[_random.Next(0, available.Count)];
    }

    private void SetValueToPoint(Point point, int value)
    {
        _board[point.x, point.y].value = value;
    }

    private int GetValueAtPoint(Point point) =>
        point.x < 0 || point.x >= Width || point.y < 0 || point.y >= Height ? -1 : _board[point.x, point.y].value;

    List<Point> IsConnected(Point point, bool main)
    {
        List<Point> connected = new List<Point>();
        int value = GetValueAtPoint(point);
        Point[] directions =
        {
            Point.Up,
            Point.Right,
            Point.Down,
            Point.Left
        };

        foreach (Point dir in directions)
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for (int i = 1; i < 3; i++)
            {
                Point check = Point.Add(point, Point.Multiply(dir, i));
                if (GetValueAtPoint(check) == value)
                {
                    line.Add(check);
                    same++;
                }
            }

            if (same > 1)
                AddPoints(ref connected, line);
        }

        for (int i = 0; i < 2; i++)
        {
            List<Point> line = new List<Point>();
            int same = 0;
            Point[] check =
            {
                Point.Add(point, directions[i]),
                Point.Add(point, directions[i + 2])
            };

            foreach (Point next in check)
            {
                if (GetValueAtPoint(next) == value)
                {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1)
                AddPoints(ref connected, line);
        }

        for (int i = 0; i < 4; i++)
        {
            List<Point> square = new List<Point>();

            int same = 0;
            int next = i + 1;
            if (next >= 4)
                next -= 4;
            Point[] check =
            {
                Point.Add(point, directions[i]),
                Point.Add(point, directions[next]),
                Point.Add(point, Point.Add(directions[i], directions[next])),
            };
            foreach (Point p in check)
            {
                if (GetValueAtPoint(p) == value)
                {
                    square.Add(p);
                    same++;
                }
            }

            if (same > 2)
                AddPoints(ref connected, square);
        }

        if (main)
            for (int i = 0; i < connected.Count; i++)
                AddPoints(ref connected, IsConnected(connected[i], false));

        return connected;
    }

    private void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach (Point p in add)
        {
            bool doAdd = true;
            for (int i = 0; i < points.Count; i++)
                if (points[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }

            if (doAdd)
                points.Add(p);
        }
    }

    public Vector2 GetPositionFromPoint(Point point)
        => new Vector2(-258 + (40 * point.x), 158 - (40 * point.y));

    public void ResetPiece(NodePiece piece)
    {
        piece.ResetPosition();
        _update.Add(piece);
    }

    public void FlipPieces(Point point1, Point point2, bool main)
    {
        if (GetValueAtPoint(point1) < 0) return;

        Node node1 = GetNodeAtPoint(point1);
        NodePiece piece1 = node1.GetPiece();

        if (GetValueAtPoint(point2) > 0)
        {
            Node node2 = GetNodeAtPoint(point2);
            NodePiece piece2 = node2.GetPiece();
            node1.SetPiece(piece2);
            node2.SetPiece(piece1);

            if (main)
                _flipped.Add(new FlippedPieces(piece1, piece2));

            _update.Add(piece1);
            _update.Add(piece2);
        }
        else ResetPiece(piece1);
    }

    private Node GetNodeAtPoint(Point point) => _board[point.x, point.y];

    private void ApplyGravityToBoard()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = Height - 1; y >= 0; y--)
            {
                Point point = new Point(x, y);
                Node node = GetNodeAtPoint(point);
                int value = GetValueAtPoint(point);
                if (value != 0) continue;
                for (int ny = y - 1; ny >= -1; ny--)
                {
                    Point next = new Point(x, ny);
                    int nextValue = GetValueAtPoint(next);
                    if (nextValue == 0) continue;
                    if (nextValue != -1)
                    {
                        Node got = GetNodeAtPoint(next);
                        NodePiece piece = got.GetPiece();
                        node.SetPiece(piece);
                        _update.Add(piece);
                        got.SetPiece(null);
                    }
                    else
                    {
                        int newValue = FillPiece();
                        NodePiece piece;
                        Point fallPoint = new Point(x, -1 - _fills[x]);
                        if (_dead.Count > 0)
                        {
                            NodePiece revived = _dead[0];
                            revived.gameObject.SetActive(true);
                            piece = revived;
                            _dead.RemoveAt(0);
                        }
                        else
                        {
                            GameObject obj = Instantiate(nodePiece, gameBoard);
                            NodePiece n = obj.GetComponent<NodePiece>();
                            piece = n;
                        }

                        piece.Initialize(newValue, point, pieces[newValue - 1]);
                        piece.rect.anchoredPosition = GetPositionFromPoint(fallPoint);
                        Node hole = GetNodeAtPoint(point);
                        hole.SetPiece(piece);
                        ResetPiece(piece);

                        _fills[x]++;
                    }

                    break;
                }
            }
        }
    }

    private void KillPiece(Point p)
    {
        List<KilledPiece> available = new List<KilledPiece>();
        for (int i = 0; i < _killed.Count; i++)
            if (!_killed[i].falling)
                available.Add(_killed[i]);

        KilledPiece set;
        if (available.Count > 0)
            set = available[0];
        else
        {
            GameObject kill = Instantiate(killedPiece, killedBoard);
            KilledPiece kPiece = kill.GetComponent<KilledPiece>();
            set = kPiece;
            _killed.Add(kPiece);
        }

        int value = GetValueAtPoint(p) - 1;
        if(set != null && value >= 0 && value < pieces.Length)
            set.Initialize(pieces[value], GetPositionFromPoint(p));
    }
}

[Serializable]
public class Node
{
    /* 0 - blank
     1 - Mercury
     2 - Neptune
     3 - Saturn
     4 - Mars
     5 - Asteroid
     6 - Black hole
     */
    public int value;
    public Point index;
    private NodePiece _piece;

    public Node(int value, Point index)
    {
        this.value = value;
        this.index = index;
    }

    public void SetPiece(NodePiece piece)
    {
        _piece = piece;
        value = piece == null ? 0 : piece.value;
        if (piece == null) return;
        piece.SetIndex(index);
    }

    public NodePiece GetPiece() => _piece;
}

[Serializable]
public class FlippedPieces
{
    public NodePiece one;
    public NodePiece two;

    public FlippedPieces(NodePiece one, NodePiece two)
    {
        this.one = one;
        this.two = two;
    }

    public NodePiece GetOtherPiece(NodePiece piece)
    {
        if (piece == one)
            return two;
        else if (piece == two)
            return one;
        else
            return null;
    }
}