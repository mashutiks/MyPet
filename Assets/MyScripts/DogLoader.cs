using UnityEngine;
using TMPro;
using Cinemachine;

public class DogLoader : MonoBehaviour
{
    public GameObject[] dogs; // Массив собак, которые могут быть выбраны
    public TMP_Text dogNameText; // Текстовое поле для имени собаки
    public CinemachineVirtualCamera virtualCamera; // Ссылка на камеру Cinemachine

    void Start()
    {
        // Получаем ID выбранной собаки
        string selectedDogID = PlayerPrefs.GetString("SelectedDogID", "");
        Debug.Log("Загружаем ID собаки: " + selectedDogID);

        // Получаем имя собаки
        string dogName = PlayerPrefs.GetString("SelectedDogName", "Безымянный");

        // Перебираем всех собак, активируя только выбранную
        foreach (GameObject dog in dogs)
        {
            if (dog.name == selectedDogID)
            {
                dog.SetActive(true);

                // Настройка камеры на выбранную собаку
                if (virtualCamera != null)
                {
                    virtualCamera.Follow = dog.transform;
                    virtualCamera.LookAt = dog.transform;
                }
            }
            else
            {
                dog.SetActive(false);
            }
        }

        // Обновляем текст с именем собаки, если текстовое поле задано
        if (dogNameText != null)
        {
            dogNameText.text = "Меня зовут " + dogName;
        }
    }
}
