using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoPlayground : MonoBehaviour
{
    public GameObject Dog; // ���� ������
    public Button PlaygroundButton; // ������ "������� ��������"
    void Start()
    {
        if (PlaygroundButton != null)
        {
            PlaygroundButton.onClick.AddListener(DownloadPlayground);
        }
        else
        {
            Debug.LogWarning("������ '������� ��������' �� ���������!");
        }
    }
    void DownloadPlayground()
    {
        SceneManager.LoadScene("Playground"); // ��������� ����� �������� ��������
    }
}