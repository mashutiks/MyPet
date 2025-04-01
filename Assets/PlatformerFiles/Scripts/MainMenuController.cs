using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuController : MonoBehaviour
{
    // �������� ������� ������ ��� ������ �����
    [SerializeField] private string firstLevelName = "1";

    // ������� ��� ������ "������"
    public void StartGame()
    {
        // ������� ���������� ����������� ������ � �������� ���� � ������� ������
        PlayerPrefs.DeleteKey("LastLevel");
        SceneManager.LoadScene(firstLevelName); // ��������� ����� ������� ������, �������� �������� ������� � firstLevelName 
    }

    // ������� ��� ������ "����������"
    public void ContinueGame()
    {
        // ���������, ���� �� ���������� �������
        if (PlayerPrefs.HasKey("LastLevel"))
        {
            string lastLevel = PlayerPrefs.GetString("LastLevel");
            // �������� �������� ���������� ������, ����������� � PlayerPrefs, � ������� � ����� ������.
            SceneManager.LoadScene(lastLevel);
        }
        else
        {
            // ���� ��� ����������� ������, �������� ���� � ������� ������
            StartGame(); //�����,������������ ���������� (��������� ���� � ������ ������)
        }
    }

    // ������� ��� ������ "�����"
    public void ExitGame()
    {
        // ����� �� ����
        Application.Quit();
    }
}


