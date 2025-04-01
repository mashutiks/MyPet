using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    // ���� ��� �������� ����� �������� ����
    [SerializeField] private string mainMenuSceneName = "Menu";

    void Update()
    {
        // ��������, ���� �� ������ ������� Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ��������� ������� �����, ����� ����� ��������� �� ��
            PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);
            // ���������� ��������� � PlayerPrefs, ����� ��������� ������ ���� ��� �����������
            PlayerPrefs.Save();

            // ��������� � ������� ����
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
