using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ActionButton : MonoBehaviour
{
    [SerializeField] private Image iconSprite;
    [SerializeField] private Sprite actionSprite;

    [SerializeField] private TMP_Text txtCost;
    [SerializeField] private float actionCost = 100f;

    private void OnValidate()
    {
        SetActionSprite(actionSprite);
        SetActionCost(actionCost);
    }

    public void SetActionSprite(Sprite actionSprite)
    {
        if (actionSprite == null) return;

        this.actionSprite = actionSprite;
        iconSprite.sprite = actionSprite;
    }

    public void SetActionCost(float cost)
    {
        actionCost = cost;
        txtCost.text = cost.ToString();
    }
}
