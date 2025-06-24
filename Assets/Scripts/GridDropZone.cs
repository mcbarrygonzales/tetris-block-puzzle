using UnityEngine;
using UnityEngine.EventSystems;

public class GridDropZone : MonoBehaviour, IDropHandler
{
    public GridSystem gridSystem;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;
        if (draggedObject == null)
        {
            Debug.Log("No object was dragged.");
            return;
        }

        BlockShape shape = draggedObject.GetComponent<BlockShape>();
        if (shape == null)
        {
            Debug.Log("Dragged object is not a BlockShape.");
            return;
        }

        Debug.Log("Trying to place shape at pointer position.");

        bool placed = gridSystem.TryPlaceShape(shape);

        if (placed)
        {
            Debug.Log("Shape successfully placed.");
    
            BlockShape blockShape = draggedObject.GetComponent<BlockShape>();
            if (!blockShape.isPlaced)
            {
                blockShape.isPlaced = true;
                blockShape.shapeManager.OnShapePlaced(blockShape);
            }
        }

        else
        {
            Debug.Log("Shape cannot be placed — invalid area or overlap.");
        }
    }

}