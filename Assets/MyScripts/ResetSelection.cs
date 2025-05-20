using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetSelection : MonoBehaviour
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
            Debug.LogWarning("Кнопка 'Выбрать другую собаку' не назначена!");
        }
    }

    void ResetAndReturnToScene1()
    {
        PlayerPrefs.DeleteKey("DogSelected"); 
        PlayerPrefs.DeleteKey("SelectedDogID"); 
        PlayerPrefs.DeleteKey("SelectedDogName"); 
        PlayerPrefs.Save(); 

        SceneManager.LoadScene("Pick_a_pet");
    }
}
