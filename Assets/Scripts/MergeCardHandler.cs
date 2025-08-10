using UnityEngine;

public class MergeCardHandler : MonoBehaviour
{
    private GridManager gridManager;

    private void Start()
    {
        gridManager = GridManager.GetInstance();
    }

    public bool TryMergeColumn(int col)
    {
        bool isMerged = false;
        for (int row = gridManager.maxRows - 1; row > 0; row--)
        {
            int val1 = gridManager.GetCardValueAt(col, row);
            int val2 = gridManager.GetCardValueAt(col, row - 1);
            if (val1 > 0 && val2 > 0)
            {
                if (val1 == val2)
                {
                    gridManager.SetCardValueAt(col, row - 1, val1 * 2);
                    gridManager.SetCardValueAt(col, row, 0);
                    isMerged = true;
                }
                else break;
            }
            
        }
        return isMerged;
    }
}
