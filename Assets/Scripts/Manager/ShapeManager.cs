using UnityEngine;
using System.Collections.Generic;

public class ShapeManager : MonoBehaviour
{
    [Header("Shape Setup")]
    [SerializeField] private Transform shapeSpawnParent;
    [SerializeField] private List<BlockShapeData> availableShapes;
    [SerializeField] private int shapeCount = 3;
    
    private List<BlockShape> currentShapes = new List<BlockShape>();
    
    public void SpawnShapes()
    {
        currentShapes.Clear();
        
        for (int i = 0; i < shapeCount; i++)
        {
            var shape = ShapePoolManager.Instance.GetShape(shapeSpawnParent);
            shape.Initialize(availableShapes[Random.Range(0, availableShapes.Count)]);
            shape.shapeManager = this;
            shape.isPlaced = false;
            currentShapes.Add(shape);
        }
    }

    public void OnShapePlaced(BlockShape placedShape)
    {
        if (!currentShapes.Contains(placedShape)) return;
        currentShapes.Remove(placedShape);
        ShapePoolManager.Instance.ReturnShape(placedShape);
        GridSystem.Instance.CheckAndClearLines();
            
        //Check if remaining shapes can be placed
        if (currentShapes.Count > 0)
        {
            List<BlockShapeData> remainingData = currentShapes.ConvertAll(s => s.shapeData);
            if (!GridSystem.Instance.CanAnyShapeBePlaced(remainingData))
            {
                GameEvents.OnGameOver?.Invoke();
                return;
            }
        }

        //Refill only if all shapes are placed
        if (currentShapes.Count == 0)
        {
            Invoke(nameof(SpawnShapes), 0.3f);
        }
    }
    
    public void ClearAllCurrentShapes()
    {
        foreach (var shape in currentShapes)
        {
            if (shape != null)
            {
                ShapePoolManager.Instance.ReturnShape(shape);
            }
        }
        currentShapes.Clear();
    }
}
