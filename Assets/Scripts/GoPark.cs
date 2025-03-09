using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NewBehaviourScript : MonoBehaviour
{
    public GameObject Dog; // наша собака
    public Button ParkButton; // кнопка "Парк"
    void Start()
    {
        if (ParkButton != null)
        {
            ParkButton.onClick.AddListener(DownloadPark);
        }
        else
        {
            Debug.LogWarning("Кнопка 'Парк' не назначена!");
        }
    }
    void DownloadPark()
    {
        SceneManager.LoadScene("Park"); // загружаем сцену парка
    }
}
