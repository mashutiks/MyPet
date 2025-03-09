using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NewBehaviourScript : MonoBehaviour
{
    public GameObject Dog; // ���� ������
    public Button ParkButton; // ������ "����"
    void Start()
    {
        if (ParkButton != null)
        {
            ParkButton.onClick.AddListener(DownloadPark);
        }
        else
        {
            Debug.LogWarning("������ '����' �� ���������!");
        }
    }
    void DownloadPark()
    {
        SceneManager.LoadScene("Park"); // ��������� ����� �����
    }
}
