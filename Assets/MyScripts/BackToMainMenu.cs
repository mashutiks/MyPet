using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMainMenu : MonoBehaviour
{
    public Button resetButton; // ������ "�����

    void Start()
    {
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetAndReturnToScene1);
        }
        else
        {
            Debug.LogWarning("������ '�����' �� ���������!");
        }
    }

    void ResetAndReturnToScene1()
    {

        SceneManager.LoadScene("Home");
    }
}
