using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    public Vector2Int gridPosition;
    public bool isOccupied;
    
    private Image image;
    
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetOccupied(bool occupied, Color? color = null)
    {
        isOccupied = occupied;

        if (occupied && color.HasValue)
        {
            image.color = color.Value;
        }
        else
        {
            image.color = new Color(1, 1, 1, 1); 
        }
    }
    
    public void AnimateClear()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.15f).setEaseOutBack().setOnComplete(() =>
        {
            SetOccupied(false);
            transform.localScale = Vector3.one;
        });
    }

}