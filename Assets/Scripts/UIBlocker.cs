using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlocker : MonoBehaviour
{
    private static UIBlocker instance;
    private List<Button> buttonsToRestore = new List<Button>();

    void Awake()
    {
        // Singleton-паттерн: один экземпляр на сцене
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // по желанию, если должен жить между сценами
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void BlockAllButtons(float duration)
    {
        if (instance != null)
            instance.StartCoroutine(instance.BlockButtonsCoroutine(duration));
        else
            Debug.LogWarning("UIBlocker instance not found in scene.");
    }

    private IEnumerator BlockButtonsCoroutine(float duration)
    {
        // Найти и отключить все кнопки, которые активны
        Button[] allButtons = FindObjectsOfType<Button>(true); // include inactive buttons
        buttonsToRestore.Clear();

        foreach (Button button in allButtons)
        {
            if (button.interactable)
            {
                button.interactable = false;
                buttonsToRestore.Add(button);
            }
        }

        yield return new WaitForSeconds(duration);

        // Восстановить только те, которые были активны до блокировки
        foreach (Button button in buttonsToRestore)
        {
            if (button != null)
                button.interactable = true;
        }
    }
}
