using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // ���������� ��� ����������� ��������; true � �������� ������
    private bool moveRight = true;
    // �������� �������� ���������
    private float speed = 2f;
    // ����� ������� �������� ���������
    private float leftBoundary = 19.10f;
    // ������ ������� �������� ���������
    private float rightBoundary = 20.10f;

    void Update()
    {
        // ��������, �������� �� ��������� ������ �������, ����� ������� ����������� �� "�����"
        if (transform.position.x >= rightBoundary)
        {
            moveRight = false;
        }
        // ��������, �������� �� ��������� ����� �������, ����� ������� ����������� �� "������"
        else if (transform.position.x <= leftBoundary)
        {
            moveRight = true;
        }

        // ��������� ����������� ��������: 1, ���� ������; -1, ���� �����
        float direction = moveRight ? 1 : -1;
        // ��������� ������� ��������� � ����������� �� �����������, �������� � ������� ����� �������
        transform.position = new Vector2(transform.position.x + direction * speed * Time.deltaTime, transform.position.y);
    }

    // ����� ����������, ����� ������ ��������� � �������� � ����������
    private void OnCollisionStay2D(Collision2D collision)
    {
        // ���������, ���� ������ ����� ��� "Player" (��������, ��������)
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // ��������� ������������� ������� ��� ������, ����� �� �������� ������ � ����������
        }
    }

    // ����� ����������, ����� ������ �������� ������� � ����������
    private void OnCollisionExit2D(Collision2D collision)
    {
        // ��������, ���� ������ ����� ��� "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // ������ ������������� �������, ����� ����� ������ �� �������� ������ � ����������
        }
    }
}
