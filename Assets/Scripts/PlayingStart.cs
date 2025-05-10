using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayingStart : MonoBehaviour
{
    public GameObject stick; // �����-�������
    private Vector3 stick_position; // ���� �������� ����� ��� ������ ����
    public Button PlayingButton; // ������ "������"
    public LineController LineController; // ������ �� ������ ��� ��������� �����
    void Start()
    {
        if (PlayingButton != null)
        {
            PlayingButton.onClick.AddListener(SetStickPosition);
        }
        else
        {
            Debug.LogWarning("������ '������' �� ���������!");
        }
    }

    void SetStickPosition()
    {
        stick_position = new Vector3(-6.69999981f, 2.55999994f, 42f);
        stick.transform.position = stick_position; // ������������� ����� ����� �������
    }

    void Update()
    {
        Vector3 speed = new Vector3(10f, 10f, 10f);
        LineController.DrawLine(stick_position, speed);
    }
}
