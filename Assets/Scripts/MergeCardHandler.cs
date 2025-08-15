using UnityEngine;

public class MergeCardHandler : MonoBehaviour
{
    private GridManager gridManager;

    private void Start()
    {
        gridManager = GridManager.GetInstance();
    }

    public bool TryMergeColumnFromBottom(int col, ref bool mergeAnyCardHasMoney)
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
                    if (gridManager.GetHasMoney(col, row) || gridManager.GetHasMoney(col, row - 1))
                    {
                        mergeAnyCardHasMoney = true;
                        gridManager.SetHasMoney(col, row, false);
                        gridManager.SetHasMoney(col, row - 1, false);
                    }
                    isMerged = true;
                }
                else break;
            }
            
        }
        return isMerged;
    }

    public bool TryMergeColumnFromTop()
    {
        bool isMerged = false;

        for (int c = 0; c < gridManager.maxColumns; ++c)
        {
            for (int r = 0; r < gridManager.maxRows - 1; ++r)
            {
                int val1 = gridManager.GetCardValueAt(c, r);
                int val2 = gridManager.GetCardValueAt(c, r + 1);
                if (val1 == 0 && val2 == 0) break;
                if (val1 == 0 && val2 != 0)
                {
                    gridManager.SetCardValueAt(c, r, val2);
                    gridManager.SetCardValueAt(c, r + 1, 0);
                    --r;
                } else if (val1 == val2)
                {
                    gridManager.SetCardValueAt(c, r, val1 * 2);
                    gridManager.SetCardValueAt(c, r + 1, 0);
                }
            }
        }

        return isMerged;
        
    }
}
