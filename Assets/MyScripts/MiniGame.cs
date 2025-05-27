using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    public Button resetButton;

    void Start()
    {
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetAndReturnToScene1);
        }
        else
        {
            Debug.LogWarning("Кнопка 'Домой' не назначена!");
        }
    }

    void ResetAndReturnToScene1()
    {

        SceneManager.LoadScene("1");
    }
}
