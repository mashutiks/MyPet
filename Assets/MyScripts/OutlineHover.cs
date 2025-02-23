using UnityEngine;
using UnityEngine.UI;

public class DogClickOutline : MonoBehaviour
{
    // Ссылка на компонент Outline (Quick Outline)
    private Outline outline;

    // Ссылка на объект панели с DogNamePanelController (панель ввода имени)
    public GameObject dogNamePanel;

    // Ссылка на кнопку "Выбрать"
    public GameObject chooseButton;

    // Флаг для отслеживания состояния выделения
    private bool isSelected = false;

    // Статическая переменная для хранения текущей выбранной собаки
    private static DogClickOutline currentSelected = null;

    void Start()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false; // Выключаем обводку по умолчанию
        }

        if (chooseButton != null)
        {
            chooseButton.SetActive(false); // Скрываем кнопку по умолчанию
        }
    }

    void OnMouseDown()
    {
        // Если панель уже активна и мы кликаем на другую собаку — скрываем панель
        if (dogNamePanel != null && dogNamePanel.activeSelf && currentSelected != null && currentSelected != this)
        {
            dogNamePanel.SetActive(false);
        }

        // Если уже выбрана другая собака, снимаем с неё выделение
        if (currentSelected != null && currentSelected != this)
        {
            currentSelected.Deselect();
        }

        // Тогглим (переключаем) выделение этой собаки
        isSelected = !isSelected;

        // Если мы только что сняли выделение с этой же собаки (isSelected стал false)
        // и при этом панель была открыта, тоже скрываем панель
        if (!isSelected && dogNamePanel != null && dogNamePanel.activeSelf && currentSelected == this)
        {
            dogNamePanel.SetActive(false);
        }

        // Настраиваем обводку
        if (outline != null)
        {
            outline.enabled = isSelected;
        }

        // Настраиваем кнопку "Выбрать"
        if (chooseButton != null)
        {
            chooseButton.SetActive(isSelected);
            if (isSelected)
            {
                // Назначаем обработчик нажатия, если собака выбрана
                Button btn = chooseButton.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners(); // Стираем старые обработчики
                    btn.onClick.AddListener(OnChooseButtonClicked);
                }
            }
        }

        // Обновляем текущую выбранную собаку
        if (isSelected)
        {
            currentSelected = this;
        }
        else
        {
            if (currentSelected == this)
                currentSelected = null;
        }
    }

    // Обработчик нажатия на кнопку "Выбрать"
    void OnChooseButtonClicked()
    {
        // Скрываем кнопку "Выбрать"
        if (chooseButton != null)
        {
            chooseButton.SetActive(false);
        }

        // Показываем панель ввода имени
        if (dogNamePanel != null)
        {
            DogNamePanelController panelController = dogNamePanel.GetComponent<DogNamePanelController>();
            if (panelController != null)
            {
                panelController.ShowPanel();
            }
            else
            {
                Debug.LogWarning("DogNamePanelController не найден на dogNamePanel");
            }
        }
    }

    // Снятие выделения с собаки
    public void Deselect()
    {
        isSelected = false;
        if (outline != null)
        {
            outline.enabled = false;
        }
        if (chooseButton != null)
        {
            chooseButton.SetActive(false);
        }
    }
}
