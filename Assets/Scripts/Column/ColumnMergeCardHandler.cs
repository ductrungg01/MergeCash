using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ColumnMergeCardHandler : MonoBehaviour
{
    private Column column;

    private void Awake()
    {
        column = GetComponent<Column>();
    }

    public bool CanMerge()
    {
        var cards = column.GetCards();
        for (int i = 0; i < cards.Count - 1; i++)
        {
            if (cards[i].Label == cards[i + 1].Label)
            {
                return true;
            }
        }
        return false;
    }

    private int GetMergeIndex()
    {
        var cards = column.GetCards();
        for (int i = 0; i < cards.Count - 1; i++)
        {
            if (cards[i].Label == cards[i + 1].Label)
            {
                return i + 1;
            }
        }

        return -1;
    }

    public IEnumerator TryMergeCoroutine()
    {
        bool merged = false;

        while (CanMerge())
        {
            merged  = true;

            int index = GetMergeIndex();

            bool done = false;

            MoveCardsUpFromIndex(index, 0.25f, () =>
            {
                // Remove the card without rearrange
                column.RemoveCard(index, false);

                // Rearrange after remove
                //column.RearrangeColumn();

                // Animate merge effect on previous card
                Card prevCard = column.GetCard(index - 1);
                if (prevCard != null)
                {
                    prevCard.UpdateNextLabel();
                    AnimateMerge(prevCard, 0.15f);
                }

                done = true;
            });

            // Wait until tween finishes
            yield return new WaitUntil(() => done);
        }

        Debug.Log("Merge finished!");
        //return merged;
    }

    private void MoveCardsUpFromIndex(int index, float duration, TweenCallback onComplete)
    {
        // Move all cards below the removed card
        for (int i = index; i < column.CardCount(); i++)
        {
            RectTransform rt = column.GetCard(i).GetComponent<RectTransform>();
            Vector2 target = rt.anchoredPosition - new Vector2(0, column.CardOffsetY);

            rt.DOAnchorPos(target, duration).SetEase(Ease.OutQuad);
        }

        // Run callback after duration
        DOVirtual.DelayedCall(duration, onComplete);
    }

    private void AnimateMerge(Card card, float duration)
    {
        Vector3 baseScale = card.transform.localScale;
        Vector3 targetScale = baseScale * 1.1f;

        card.transform.DOScale(targetScale, duration)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => card.transform.localScale = baseScale);
    }
}
