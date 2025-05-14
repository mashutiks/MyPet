using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinCounterUI : MonoBehaviour
{
    [Header("�������� UI")]
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
            coinText.text = "�� �������: " + miniGamePoints + " �����";
        }
    }

    public void ResetMiniGamePoints()
    {
        PlayerPrefs.SetInt("PointsMiniGame", 0);
        PlayerPrefs.Save();
        UpdateCoinText();
    }
}