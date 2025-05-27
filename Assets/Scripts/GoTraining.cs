using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoTraining : MonoBehaviour
{
    public GameObject Dog; // наша собака
    public Button TrainingButton; // кнопка "Дрессировать"
    void Start()
    {
        if (TrainingButton != null)
        {
            TrainingButton.onClick.AddListener(DownloadTraining);
        }
        else
        {
            Debug.LogWarning("Кнопка 'Дрессировать' не назначена!");
        }
    }
    void DownloadTraining()
    {
        SceneManager.LoadScene("Training_zone"); // загружаем сцену дрессировочной зоны
    }
}
