using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    // ����� ����������, ����� ������ ������ � ������� (��������� � ���������� IsTrigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ��������, ��� � ������� ����� ������ � ����� "Player"
        if (other.CompareTag("Player"))
        {
            // �������� ������ ������� ����� � ��������� ���������
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // ��������� ��������� �����, ��������� ������ ������� ����� + 1
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}