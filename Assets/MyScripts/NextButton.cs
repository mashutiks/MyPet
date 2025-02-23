using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Для перехода между сценами
using TMPro;

public class DogNamePanelController : MonoBehaviour
{
    // Ссылка на поле ввода имени (TMP_InputField)
    public TMP_InputField dogNameInput;

    // Ссылка на кнопку "Далее"
    public Button nextButton;

    void Start()
    {
        // Скрываем панель при запуске игры
        gameObject.SetActive(false);

        // Добавляем обработчик клика для кнопки "Далее"
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClick);
        }
    }

    // Метод для показа панели (вызывается из другого скрипта)
    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    // Метод, вызываемый при нажатии на кнопку "Далее"
    void OnNextButtonClick()
    {
        string dogName = dogNameInput.text;
        if (string.IsNullOrEmpty(dogName))
        {
            Debug.Log("Введите имя собаки!");
            return;
        }

        Debug.Log("Имя собаки: " + dogName);
        // Здесь можно сохранить имя собаки для дальнейшего использования

        // Загружаем следующую сцену. Убедитесь, что сцена добавлена в Build Settings.
        SceneManager.LoadScene("Scene2"); // Замените "NextScene" на имя нужной сцены
    }
}
