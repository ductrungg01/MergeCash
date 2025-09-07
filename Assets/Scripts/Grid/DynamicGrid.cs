using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // Makes this script run both in Play Mode and Edit Mode
[RequireComponent(typeof(GridLayoutGroup))]
public class DynamicGrid : MonoBehaviour
{
    [SerializeField] private const int COLUMN_COUNT = 4;      // Number of columns to display

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
        float totalSpacing = grid.spacing.x * (COLUMN_COUNT - 1) + grid.padding.left + grid.padding.right;

        // Calculate cell width so that exactly 'columnCount' cells fit the container width
        float cellWidth = (rectTransform.rect.width - totalSpacing) / COLUMN_COUNT;

        float height = rectTransform.rect.height;

        // Apply calculated size
        grid.cellSize = new Vector2(cellWidth, height);
    }

    void OnRectTransformDimensionsChange()
    {
        // Also update when the RectTransform changes (like resizing in the editor)
        UpdateCellSize();
    }
}
