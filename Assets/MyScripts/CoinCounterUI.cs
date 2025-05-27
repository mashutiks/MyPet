using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinCounterUI : MonoBehaviour
{
    [Header("Привязки UI")]
    public Button resetButton;
    public TMP_Text coinText;

    void Start()
    {
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetMiniGamePoints);
        }
    }

    void Update()
    {
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            int miniGamePoints = PlayerPrefs.GetInt("PointsMiniGame", 0);
            coinText.text = "Вы собрали: " + miniGamePoints + " монет";
        }
    }

    public void ResetMiniGamePoints()
    {
        PlayerPrefs.SetInt("PointsMiniGame", 0);
        PlayerPrefs.Save();
        UpdateCoinText();
    }
}