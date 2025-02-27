using UnityEngine;
using TMPro;

public class DogLoader : MonoBehaviour
{
    public GameObject[] dogs; // Массив собак, которые могут быть выбраны
    public TMP_Text dogNameText; // Текстовое поле для имени собаки

    void Start()
    {
        string selectedDogID = PlayerPrefs.GetString("SelectedDogID", "");
        Debug.Log("Загружаем ID собаки: " + selectedDogID);

        string dogName = PlayerPrefs.GetString("SelectedDogName", "Безымянный");

        foreach (GameObject dog in dogs)
        {
            if (dog.name == selectedDogID)
            {
                dog.SetActive(true);
            }
            else
            {
                dog.SetActive(false);
            }
        }

        if (dogNameText != null)
        {
            dogNameText.text = "Имя собаки: " + dogName;
        }
    }
}