using System;
using System.Collections.Generic;
using System.Linq;
using EditorAttributes;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "Prefab Brush", menuName = "Brushes/Prefab Brush")]
[CustomGridBrush(false, false, false, "Grid Objects")]
public class GridObjectBrush : GameObjectBrush
{
    [SerializeField] private GridObjectCell[] availableCells;

    [Dropdown("GetCellstNames")]
    [SerializeField] private string selectedCell;

    private string _lastSelected;

    private void OnValidate()
    {
        if (_lastSelected != selectedCell)
        {
            ChangeBrush();
            _lastSelected = selectedCell;
        }
    }

    private void ChangeBrush()
    {
        SetBrushCell(FindCell(selectedCell).Cell);
    }

    [Serializable]
    private class GridObjectCell
    {
        public string Name;

        public BrushCell[] Cell;

    }

    private string[] GetCellstNames => availableCells
        .DistinctBy(cell => cell.Name)
        .Where(cell => cell.Name != "")
        .Select(cell => cell.Name)
        .ToArray();


    private GridObjectCell FindCell(string name)
    {
        if (name is null)
            return null;
        GridObjectCell newCell = availableCells.Where(cell => cell.Name != "" || cell.Name != null).First(cell => cell.Name == name);

        return newCell is null ? null : newCell;
    }
}
