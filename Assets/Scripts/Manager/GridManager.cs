using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    public int rows = 9;
    public int cols = 9;
    public GameObject cellPrefab;
    public Transform gridParent;

    private GridCell[,] grid;
    private GridLayoutGroup gridLayout;

    private void Awake()
    {
        Instance = this;
        gridLayout = GetComponent<GridLayoutGroup>();
    }
    
    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < cols && y >= 0 && y < rows;
    }

    private Vector2 GetGridOriginTopLeft()
    {
        Vector2 cellSize = gridLayout.cellSize + gridLayout.spacing;
        var width = cols * cellSize.x;
        var height = rows * cellSize.y;
        return new Vector2(-width / 2f, height / 2f);
    }

    private Vector2Int GetGridCoordinatesFromLocalPoint(Vector2 localPoint)
    {
        Vector2 cellSize = gridLayout.cellSize + gridLayout.spacing;
        Vector2 offset = localPoint - GetGridOriginTopLeft();
        var x = Mathf.FloorToInt(offset.x / cellSize.x);
        var y = Mathf.FloorToInt(-offset.y / cellSize.y);
        return new Vector2Int(x, y);
    }

    private Vector2Int GetShapeBounds(BlockShapeData shapeData)
    {
        var maxX = 0;
        var maxY = 0;

        foreach (var cell in shapeData.occupiedCells)
        {
            if (cell.x > maxX) maxX = cell.x;
            if (cell.y > maxY) maxY = cell.y;
        }

        return new Vector2Int(maxX, maxY);
    }

    private bool CanPlaceShapeAt(BlockShapeData shapeData, int baseX, int baseY)
    {
        foreach (var cell in shapeData.occupiedCells)
        {
            var x = baseX + cell.x;
            var y = baseY + cell.y;

            if (!IsInBounds(x, y) || grid[x, y].isOccupied)
                return false;
        }

        return true;
    }
    
    public void InitializeGrid()
    {
        grid = new GridCell[rows, cols];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                GameObject obj = Instantiate(cellPrefab, gridParent);
                GridCell cell = obj.GetComponent<GridCell>();
                cell.gridPosition = new Vector2Int(x, y);
                cell.SetOccupied(false);
                grid[x, y] = cell;
            }
        }
    }

    public bool TryPlaceShape(BlockShape shape)
    {
        Vector2 screenPos = shape.transform.position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            screenPos,
            null,
            out Vector2 localPoint
        );

        Vector2Int baseCoords = GetGridCoordinatesFromLocalPoint(localPoint);
        var baseX = baseCoords.x;
        var baseY = baseCoords.y;

        foreach (var cell in shape.shapeData.occupiedCells)
        {
            var x = baseX + cell.x;
            var y = baseY + cell.y;

            if (!IsInBounds(x, y) || grid[x, y].isOccupied)
            {
                return false;
            }
        }

        Color shapeColor = shape.GetComponentInChildren<Image>().color;
        foreach (var cell in shape.shapeData.occupiedCells)
        {
            var x = baseX + cell.x;
            var y = baseY + cell.y;
            grid[x, y].SetOccupied(true, shapeColor);
        }

        return true;
    }

    
    
    public void CheckAndClearLines()
    {
        List<int> fullRows = new List<int>();
        List<int> fullCols = new List<int>();

        // Check full rows
        for (int y = 0; y < rows; y++)
        {
            bool isFull = true;
            for (int x = 0; x < cols; x++)
            {
                if (!grid[x, y].isOccupied)
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull) fullRows.Add(y);
        }

        // Check full columns
        for (int x = 0; x < cols; x++)
        {
            bool isFull = true;
            for (int y = 0; y < rows; y++)
            {
                if (!grid[x, y].isOccupied)
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull) fullCols.Add(x);
        }

        // Animate clear full rows
        foreach (int y in fullRows)
        {
            for (int x = 0; x < cols; x++)
            {
                grid[x, y].AnimateClear();
            }
        }

        // Animate clear full columns
        foreach (int x in fullCols)
        {
            for (int y = 0; y < rows; y++)
            {
                grid[x, y].AnimateClear();
            }
        }

        var clearedLines = fullRows.Count + fullCols.Count;
        if (clearedLines > 0)
        {
            GameEvents.OnLinesCleared?.Invoke(clearedLines);
        }
    }

    
    public bool CanAnyShapeBePlaced(List<BlockShapeData> shapes)
    {
        foreach (var shapeData in shapes)
        {
            // Get shape bounds
            Vector2Int maxOffset = GetShapeBounds(shapeData);

            for (int y = 0; y <= rows - maxOffset.y - 1; y++)
            {
                for (int x = 0; x <= cols - maxOffset.x - 1; x++)
                {
                    if (CanPlaceShapeAt(shapeData, x, y))
                        return true;
                }
            }
        }

        return false;
    }
    
    public void ResetGrid()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                grid[x, y].SetOccupied(false);
            }
        }
    }
    
}
