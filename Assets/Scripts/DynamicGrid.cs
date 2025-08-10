using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // Makes this script run both in Play Mode and Edit Mode
[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicGrid : MonoBehaviour
{
    public int columnCount = 4;      // Number of columns to display
    public float spacingX = 5f;      // Horizontal spacing between cells
    public float spacingY = 3f;      // Vertical spacing between cells
    public float aspectRatio = 1.2f; // Height = Width * aspectRatio

    private GridLayoutGroup grid;
    private RectTransform rectTransform;

    void Awake()
    {
        // Get required components
        grid = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // In Edit Mode, Update is called when something changes in the scene
        if (!Application.isPlaying)
        {
            UpdateCellSize();
        }
    }

    void Start()
    {
        // Update once on Start in Play Mode
        if (Application.isPlaying)
        {
            UpdateCellSize();
        }
    }

    void UpdateCellSize()
    {
        if (grid == null || rectTransform == null) return;

        // Total horizontal space taken by spacing and padding
        float totalSpacing = grid.spacing.x * (columnCount - 1) + grid.padding.left + grid.padding.right;

        // Calculate cell width so that exactly 'columnCount' cells fit the container width
        float cellWidth = (rectTransform.rect.width - totalSpacing) / columnCount;

        // Calculate cell height based on the desired aspect ratio
        float cellHeight = cellWidth * aspectRatio;

        // Apply calculated size
        grid.cellSize = new Vector2(cellWidth, cellHeight);
    }

    void OnRectTransformDimensionsChange()
    {
        // Also update when the RectTransform changes (like resizing in the editor)
        UpdateCellSize();
    }
}
