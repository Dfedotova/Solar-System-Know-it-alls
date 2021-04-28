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

    public RowData[] rows = new RowData[14]; 
}