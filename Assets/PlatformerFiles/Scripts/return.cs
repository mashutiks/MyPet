using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    // поле для названия сцены главного меню
    [SerializeField] private string mainMenuSceneName = "Menu";

    void Update()
    {
        // проверка, была ли нажата клавиша Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // сохраняем текущую сцену, чтобы потом вернуться на неё
            PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);
            // сохранение изменений в PlayerPrefs, чтобы сохранить данные даже при перезапуске
            PlayerPrefs.Save();

            // переходим в главное меню
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
