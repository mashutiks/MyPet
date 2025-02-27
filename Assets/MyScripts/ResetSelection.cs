using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetSelection : MonoBehaviour
{
    public Button resetButton; // ������ "������� ������ ������"

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
    }

    void ResetAndReturnToScene1()
    {
        PlayerPrefs.DeleteKey("DogSelected"); // ������� ���������� � ������ ������
        PlayerPrefs.DeleteKey("DogID"); // ������� ID ������
        PlayerPrefs.DeleteKey("DogName"); // ������� ��� ������
        PlayerPrefs.Save(); // ��������� ���������

        SceneManager.LoadScene("Pick_a_pet"); // ������������ �� ����� ������
    }
}
