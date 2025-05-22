using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            string levelKey = $"MiniGame_Level{currentSceneIndex}_Completed";
            int currentCount = PlayerPrefs.GetInt(levelKey, 0);
            currentCount++;
            PlayerPrefs.SetInt(levelKey, currentCount);
            PlayerPrefs.Save();

            Debug.Log($"Уровень {currentSceneIndex} пройден {currentCount} раз.");

            SceneManager.LoadScene(currentSceneIndex + 1);

            if (currentSceneIndex == 1)
                AchievemenetManager.Instance.EarnAchievement("Прыжок в небеса");
            if (currentSceneIndex == 2)
                AchievemenetManager.Instance.EarnAchievement("Уклонение от пернатых");
            if (currentSceneIndex == 3)
                AchievemenetManager.Instance.EarnAchievement("Кошачьи ловушки");
            if (currentSceneIndex == 4)
                AchievemenetManager.Instance.EarnAchievement("На волоске от победы");
            if (currentSceneIndex == 5)
                AchievemenetManager.Instance.EarnAchievement("Победитель птиц и кошек");
        }
    }

}