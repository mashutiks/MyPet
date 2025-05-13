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
    public Vector3 initial_scale; // ��������� ������ �����
    private string dog_id; // ���������� � ID ������
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

        CheckItemAvailability();
    }


    void SetStickPosition()
    {
        dog_id = PlayerPrefs.GetString("SelectedDogID", ""); // �����, ����� � ��� ������
        stick.transform.SetParent(null); // ���������� ����� �������
        stick.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        if (dog_id == "pug" || dog_id == "germanshepherd") // � ����������� �� ���� ������ ���� ����������� ����� �����
        {
            stick.transform.localScale = initial_scale;
            stick.transform.localScale = new Vector3(initial_scale.x, initial_scale.y * 1.5f, initial_scale.z);
        }
        stick_position = new Vector3(-6.69999981f, 2.55999994f, 40f);
        stick.transform.position = stick_position; // ������������� ����� ����� �������
        PlayingButton.interactable = false; // ��������� ������
    }
    void CheckItemAvailability()
    {
        int stick = PlayerPrefs.GetInt("Item_Stick", 0);
        int bone = PlayerPrefs.GetInt("Item_Bone", 0);
        int fish = PlayerPrefs.GetInt("Item_Fish", 0);

        bool hasItem = (stick == 1 || bone == 1 || fish == 1);

        if (PlayingButton != null)
        {
            PlayingButton.interactable = hasItem;
        }
    }
}



