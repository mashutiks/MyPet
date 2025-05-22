using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DogNamePanelController : MonoBehaviour
{
    public TMP_InputField dogNameInput;
    public Button nextButton;

    private string selectedDogID;

    void Start()
    {
        gameObject.SetActive(false);

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClick);
        }
    }

    public void ShowPanel(string dogID)
    {
        selectedDogID = dogID; 
        gameObject.SetActive(true);
    }

    void OnNextButtonClick()
    {
        string dogName = dogNameInput.text;

        if (string.IsNullOrEmpty(dogName))
        {
            Debug.Log("������� ��� ������!");
            return;
        }

        PlayerPrefs.SetString("SelectedDogName", dogName);
        PlayerPrefs.SetString("SelectedDogID", selectedDogID);

        PlayerPrefs.SetInt("Dog_Choice_Logged", 0); // ����� ����� ����� ����� ���������� �����


        PlayerPrefs.Save();
        Debug.Log($"��������� ��� ������: {dogName}, ID: {selectedDogID}");

        SceneManager.LoadScene("Home");
    }
}
