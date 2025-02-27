using UnityEngine;
using TMPro;

public class DogLoader : MonoBehaviour
{
    public GameObject[] dogs; // ������ �����, ������� ����� ���� �������
    public TMP_Text dogNameText; // ��������� ���� ��� ����� ������

    void Start()
    {
        string selectedDogID = PlayerPrefs.GetString("SelectedDogID", "");
        Debug.Log("��������� ID ������: " + selectedDogID);

        string dogName = PlayerPrefs.GetString("SelectedDogName", "����������");

        foreach (GameObject dog in dogs)
        {
            if (dog.name == selectedDogID)
            {
                dog.SetActive(true);
            }
            else
            {
                dog.SetActive(false);
            }
        }

        if (dogNameText != null)
        {
            dogNameText.text = "��� ������: " + dogName;
        }
    }
}