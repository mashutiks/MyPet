using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    private Vector3 startPosition; //��������� ������� ���������

    void Start()
    {
        // ��������� ��������� ������� ���������
        startPosition = transform.position;
    }

    void Update()
    {
        // ��������, ���� �� �������� ���� ������������ ������
        if (transform.position.y < -10) 
        {
            ResetPosition();
        }
    }

    // ����������� ��������� �� ��������� �������
    void ResetPosition()
    {
        transform.position = startPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy")) 
        {
            ResetPosition();
        }
    }
}
