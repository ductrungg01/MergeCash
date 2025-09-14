using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ShopListItemDynamicLayout : GridLayoutGroup
{
    [SerializeField, Tooltip("Width / Height ratio of each cell")]
    private float aspectRatio = 1f; // width/height ratio

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        float totalWidth = rectTransform.rect.width;
        float cellWidth = (totalWidth - (spacing.x * (constraintCount - 1)) - padding.left - padding.right) / constraintCount;
        float cellHeight = cellWidth / aspectRatio;

        cellSize = new Vector2(cellWidth, cellHeight);
    }
}
