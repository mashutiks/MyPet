using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuController : MonoBehaviour
{
    // название первого уровня для кнопки старт
    [SerializeField] private string firstLevelName = "1";

    // функция для кнопки "Начать"
    public void StartGame()
    {
        // очищаем сохранение предыдущего уровня и начинаем игру с первого уровня
        PlayerPrefs.DeleteKey("LastLevel");
        SceneManager.LoadScene(firstLevelName); // загружаем сцену первого уровня, название которого указано в firstLevelName 
    }

    // функция для кнопки "Продолжить"
    public void ContinueGame()
    {
        // проверяем, есть ли сохранённый уровень
        if (PlayerPrefs.HasKey("LastLevel"))
        {
            string lastLevel = PlayerPrefs.GetString("LastLevel");
            // загрузка названия последнего уровня, сохранённого в PlayerPrefs, и переход к этому уровню.
            SceneManager.LoadScene(lastLevel);
        }
        else
        {
            // если нет сохранённого уровня, начинаем игру с первого уровня
            StartGame(); //метод,сбрасывающий сохранение (запускает игру с самого начала)
        }
    }

    // функция для кнопки "Выход"
    public void ExitGame()
    {
        // ыыход из игры
        Application.Quit();
    }
}


