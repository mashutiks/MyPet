using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoTraining : MonoBehaviour
{
    public GameObject Dog; // ���� ������
    public Button TrainingButton; // ������ "������������"
    void Start()
    {
        if (TrainingButton != null)
        {
            TrainingButton.onClick.AddListener(DownloadTraining);
        }
        else
        {
            Debug.LogWarning("������ '������������' �� ���������!");
        }
    }
    void DownloadTraining()
    {
        SceneManager.LoadScene("Training_zone"); // ��������� ����� �������������� ����
    }
}
