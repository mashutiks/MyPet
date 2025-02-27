using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ��� �������� ����� �������
using TMPro;

public class DogNamePanelController : MonoBehaviour
{
    public TMP_InputField dogNameInput; // ���� ����� ����� ������
    public Button nextButton; // ������ "�����"

    private string selectedDogID; // ������ ID ������

    void Start()
    {
        gameObject.SetActive(false); // �������� ������ ��� ������

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClick);
        }
    }

    // ����� ��� ������ ������ � ���������� ID ������
    public void ShowPanel(string dogID)
    {
        selectedDogID = dogID; // ��������� ���������� ID ������
        gameObject.SetActive(true);
    }

    // ����� ��������� ������� ������ "�����"
    void OnNextButtonClick()
    {
        string dogName = dogNameInput.text;

        if (string.IsNullOrEmpty(dogName))
        {
            Debug.Log("������� ��� ������!");
            return;
        }

        PlayerPrefs.SetString("SelectedDogName", dogName);
        PlayerPrefs.SetString("SelectedDogID", selectedDogID); // ������ ��������� ID ������
        PlayerPrefs.Save();

        Debug.Log($"��������� ��� ������: {dogName}, ID: {selectedDogID}");

        // ��������� ��������� �����
        SceneManager.LoadScene("Scene2"); // ���������, ��� ����� ��������� � Build Settings
    }
}
