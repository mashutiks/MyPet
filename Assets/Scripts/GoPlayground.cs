using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoPlayground : MonoBehaviour
{
    public GameObject Dog; // наша собака
    public Button PlaygroundButton; // кнопка "Собачья площадка"
    void Start()
    {
        if (PlaygroundButton != null)
        {
            PlaygroundButton.onClick.AddListener(DownloadPlayground);
        }
        else
        {
            Debug.LogWarning("Кнопка 'Собачья площадка' не назначена!");
        }
    }
    void DownloadPlayground()
    {
        SceneManager.LoadScene("Playground"); // загружаем сцену собачьей площадки
    }
}