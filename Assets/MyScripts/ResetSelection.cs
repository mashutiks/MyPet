using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetSelection : MonoBehaviour
{
    public Button resetButton; // Кнопка "Выбрать другую собаку"

    void Start()
    {
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetAndReturnToScene1);
        }
        else
        {
            Debug.LogWarning("Кнопка 'Выбрать другую собаку' не назначена!");
        }
    }

    void ResetAndReturnToScene1()
    {
        PlayerPrefs.DeleteKey("DogSelected"); // Удаляем информацию о выборе собаки
        PlayerPrefs.DeleteKey("DogID"); // Удаляем ID собаки
        PlayerPrefs.DeleteKey("DogName"); // Удаляем имя собаки
        PlayerPrefs.Save(); // Сохраняем изменения

        SceneManager.LoadScene("Pick_a_pet"); // Возвращаемся на сцену выбора
    }
}
