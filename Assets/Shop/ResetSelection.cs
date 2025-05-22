using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetSelection : MonoBehaviour
{
    public Button resetButton;
    public GameObject insufficientFundsPanel;

    private const int ResetCost = 1000;

    void Start()
    {
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetAndReturnToScene1);
        }
        else
        {
            Debug.LogWarning("������ '������� ������ ������' �� ���������!");
        }

        if (insufficientFundsPanel != null)
        {
            insufficientFundsPanel.SetActive(false);
        }
    }

    void ResetAndReturnToScene1()
    {
        int points = PlayerPrefs.GetInt("Points", 0);

        if (points < ResetCost)
        {
            Debug.Log("������������ ����� ��� ������ ������ ������!");
            if (insufficientFundsPanel != null)
                StartCoroutine(ShowInsufficientFundsMessage());
            return;
        }

        points -= ResetCost;
        PlayerPrefs.SetInt("Points", points);

        PlayerPrefs.DeleteKey("DogSelected");
        PlayerPrefs.DeleteKey("SelectedDogID");
        PlayerPrefs.DeleteKey("SelectedDogName");
        PlayerPrefs.Save();

        Debug.Log("����� ������ �������. 1000 ����� �����.");
        SceneManager.LoadScene("Pick_a_pet");
    }

    System.Collections.IEnumerator ShowInsufficientFundsMessage()
    {
        if (insufficientFundsPanel != null)
        {
            insufficientFundsPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            insufficientFundsPanel.SetActive(false);
        }
    }
}
