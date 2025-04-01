using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    // метод вызываетс€, когда объект входит в триггер (коллайдер с включЄнным IsTrigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // проверка, что в триггер попал объект с тегом "Player"
        if (other.CompareTag("Player"))
        {
            // получили индекс текущей сцены и загружаем следующую
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // загружаем следующую сцену, использу€ индекс текущей сцены + 1
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}