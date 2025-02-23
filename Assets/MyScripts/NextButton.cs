using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ��� �������� ����� �������
using TMPro;

public class DogNamePanelController : MonoBehaviour
{
    // ������ �� ���� ����� ����� (TMP_InputField)
    public TMP_InputField dogNameInput;

    // ������ �� ������ "�����"
    public Button nextButton;

    void Start()
    {
        // �������� ������ ��� ������� ����
        gameObject.SetActive(false);

        // ��������� ���������� ����� ��� ������ "�����"
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClick);
        }
    }

    // ����� ��� ������ ������ (���������� �� ������� �������)
    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    // �����, ���������� ��� ������� �� ������ "�����"
    void OnNextButtonClick()
    {
        string dogName = dogNameInput.text;
        if (string.IsNullOrEmpty(dogName))
        {
            Debug.Log("������� ��� ������!");
            return;
        }

        Debug.Log("��� ������: " + dogName);
        // ����� ����� ��������� ��� ������ ��� ����������� �������������

        // ��������� ��������� �����. ���������, ��� ����� ��������� � Build Settings.
        SceneManager.LoadScene("Scene2"); // �������� "NextScene" �� ��� ������ �����
    }
}
