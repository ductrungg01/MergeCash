using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TMP_Text scoreText; // keo UI Text vao trong Inspector
    public TMP_Text moneyText;

    public int moneyAmount = 1; // tuy chinh so tien cong moi lan
    public int score = 0;
    private int money = 0;

    public int HighestScore =0;

    private void Awake()
    {
        // tao singleton
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // ham cong diem
    public void AddScore(int mergedValue, bool hasMoney)
    {
        // cong diem theo quy tac:
        // moi lan gia tri x2 thi diem +10
        // vi du: 2+2 -> gia tri moi la 4 => diem +20
        //        4+4 -> gia tri moi la 8 => diem +30
        //        8+8 -> gia tri moi la 16 => diem +40
        // cong thuc: diem = (log2(mergedValue) - 1) * 10

        int add = (int)(Mathf.Log(mergedValue, 2) - 1) * 10;
        score += add;

        // neu card co tien thi cong them vao money
        if (hasMoney)
        {
            money += moneyAmount; // o day tuy luat cho moi lan merge +1 dong tien
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + score.ToString();
        moneyText.text = "Money: " + money.ToString();
    }
}
