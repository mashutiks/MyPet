using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class go_to_main_menu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "Menu"; // название главного меню
    // возвращение в главное меню
    public void ReturnToMainMenu()
    {
        // загрузка сцены меню
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
