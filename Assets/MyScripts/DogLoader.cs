using UnityEngine;
using TMPro;
using Cinemachine;

public class DogLoader : MonoBehaviour
{
    public GameObject[] dogs; // ������ �����, ������� ����� ���� �������
    public TMP_Text dogNameText; // ��������� ���� ��� ����� ������
    public CinemachineVirtualCamera virtualCamera; // ������ �� ������ Cinemachine

    void Start()
    {
        // �������� ID ��������� ������
        string selectedDogID = PlayerPrefs.GetString("SelectedDogID", "");
        Debug.Log("��������� ID ������: " + selectedDogID);

        // �������� ��� ������
        string dogName = PlayerPrefs.GetString("SelectedDogName", "����������");

        // ���������� ���� �����, ��������� ������ ���������
        foreach (GameObject dog in dogs)
        {
            if (dog.name == selectedDogID)
            {
                dog.SetActive(true);

                // ��������� ������ �� ��������� ������
                if (virtualCamera != null)
                {
                    virtualCamera.Follow = dog.transform;
                    virtualCamera.LookAt = dog.transform;
                }
            }
            else
            {
                dog.SetActive(false);
            }
        }

        // ��������� ����� � ������ ������, ���� ��������� ���� ������
        if (dogNameText != null)
        {
            dogNameText.text = "���� ����� " + dogName;
        }
    }
}
