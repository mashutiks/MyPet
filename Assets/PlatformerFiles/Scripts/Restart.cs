using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Restart : MonoBehaviour
{
    private Vector3 startPosition; //начальная позиция персонажа

    void Start()
    {
        // сохраняем начальную позицию персонажа
        startPosition = transform.position;
    }

    void Update()
    {
        // проверка, упал ли персонаж ниже определенной высоты
        if (transform.position.y < -10) 
        {
            RegisterDeath();
            ResetPosition();
        }
    }

    // возвращение персонажа на начальную позицию
    void ResetPosition()
    {
        transform.position = startPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy")) 
        {
            RegisterDeath();
            ResetPosition();
        }
    }

    void RegisterDeath()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        string statKey = GetDeathStatKey(sceneName);
        if (!string.IsNullOrEmpty(statKey))
        {
            int currentCount = PlayerPrefs.GetInt(statKey, 0);
            PlayerPrefs.SetInt(statKey, currentCount + 1);
            PlayerPrefs.Save();

            Debug.Log($"[DeathTracker] {statKey} incremented to {currentCount + 1}");
        }
        else
        {
            Debug.LogWarning("[DeathTracker] Неизвестная сцена: статистика не обновлена");
        }
    }
    string GetDeathStatKey(string sceneName)
    {
        switch (sceneName)
        {
            case "1": return "MiniGame_Level1_Deaths";
            case "2": return "MiniGame_Level2_Deaths";
            case "3": return "MiniGame_Level3_Deaths";
            case "4": return "MiniGame_Level4_Deaths";
            case "5": return "MiniGame_Level5_Deaths";
            default: return null;
        }
    }
}
