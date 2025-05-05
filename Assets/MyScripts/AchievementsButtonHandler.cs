using UnityEngine;
using UnityEngine.UI;

public class AchievementsButtonHandler : MonoBehaviour
{
    public GameObject achievementMenu; // Ссылка на панель достижений

    // Метод, который будет вызываться при нажатии на кнопку
    public void OpenAchievements()
    {
        // Проверяем, активна ли панель
        bool isActive = achievementMenu.activeSelf;

        // Переключаем состояние панели
        achievementMenu.SetActive(!isActive);
    }
}
