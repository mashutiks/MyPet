using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class go_to_main_menu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "Menu"; // �������� �������� ����
    // ����������� � ������� ����
    public void ReturnToMainMenu()
    {
        // �������� ����� ����
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
