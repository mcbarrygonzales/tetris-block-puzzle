using UnityEngine;
using UnityEngine.UI;

public class BlockShape : MonoBehaviour
{
    public BlockShapeData shapeData;
    public GameObject tilePrefab;
    public ShapeManager shapeManager;
    public bool isPlaced = false;
    public float tileSpacing = 150f;

    public void Initialize(BlockShapeData data)
    {
        shapeData = data;

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach (Vector2Int cellPos in shapeData.occupiedCells)
        {
            var tile = Instantiate(tilePrefab, transform);
            tile.GetComponent<Image>().color = shapeData.shapeColor;

            var rt = tile.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(cellPos.x * tileSpacing, -cellPos.y * tileSpacing);
        }

        var rect = GetComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f, 0.5f);
    }
}