using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Для перехода между сценами
using TMPro;

public class DogNamePanelController : MonoBehaviour
{
    public TMP_InputField dogNameInput; // Поле ввода имени собаки
    public Button nextButton; // Кнопка "Далее"

    private string selectedDogID; // Хранит ID собаки

    void Start()
    {
        gameObject.SetActive(false); // Скрываем панель при старте

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClick);
        }
    }

    // Метод для показа панели и сохранения ID собаки
    public void ShowPanel(string dogID)
    {
        selectedDogID = dogID; // Сохраняем переданный ID собаки
        gameObject.SetActive(true);
    }

    // Метод обработки нажатия кнопки "Далее"
    void OnNextButtonClick()
    {
        string dogName = dogNameInput.text;

        if (string.IsNullOrEmpty(dogName))
        {
            Debug.Log("Введите имя собаки!");
            return;
        }

        PlayerPrefs.SetString("SelectedDogName", dogName);
        PlayerPrefs.SetString("SelectedDogID", selectedDogID); // Теперь сохраняем ID собаки
        PlayerPrefs.Save();

        Debug.Log($"Сохранено имя собаки: {dogName}, ID: {selectedDogID}");

        // Загружаем следующую сцену
        SceneManager.LoadScene("Scene2"); // Убедитесь, что сцена добавлена в Build Settings
    }
}
