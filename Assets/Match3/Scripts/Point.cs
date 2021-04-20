using UnityEngine;
using System;

[Serializable]
public class Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void Multiply(int n)
    {
        x *= n;
        y *= n;
    }

    public void Add(Point p)
    {
        x += p.x;
        y += p.y;
    }

    public Vector2 ToVector() => new Vector2(x, y);

    public static Point FromVector(Vector2 vector) => new Point((int) vector.x, (int) vector.y);

    public static Point FromVector(Vector3 vector) => new Point((int) vector.x, (int) vector.y);

    public static Point Multiply(Point point, int n) => new Point(point.x * n, point.y * n);

    public static Point Clone(Point point) => new Point(point.x, point.y);

    public static Point Add(Point p1, Point p2) => new Point(p1.x + p2.x, p1.y + p2.y);

    public bool Equals(Point point) => x == point.x && y == point.y;

    public static Point Zero => new Point(0, 0);

    public static Point One => new Point(1, 1);

    public static Point Up => new Point(0, 1);

    public static Point Down => new Point(0, -1);

    public static Point Right => new Point(1, 0);

    public static Point Left => new Point(-1, 0);
}