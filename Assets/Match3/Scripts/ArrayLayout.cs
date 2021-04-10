using UnityEngine;
using System;

[Serializable]
public class ArrayLayout 
{
    [Serializable]
    public struct RowData
    {
        public bool[] row;
    }

    public Grid grid;
    public RowData[] rows = new RowData[14]; // Grid 7x7
}
