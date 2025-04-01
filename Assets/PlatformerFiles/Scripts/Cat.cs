using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public float speed = 2f; // �������� �������� ����
    public float runRange = 3f; // �������� ���� (����� � ������)
    private Vector3 startPosition; // ��������� �������
    private bool movingLeft = true; // ����������� ��������

    private SpriteRenderer sprite; // ��������� SpriteRenderer

    void Start()
    {
        startPosition = transform.position; // ��������� ��������� �������
        sprite = GetComponentInChildren<SpriteRenderer>(); // �������� ��������� SpriteRenderer
    }

    void Update()
    {
        MoveCat();
    }

    private void MoveCat()
    {
        // ���������� ����������� ��������
        if (movingLeft)
        {
            transform.position -= Vector3.right * speed * Time.deltaTime;

            // ���������, �������� �� �� ����� �������
            if (transform.position.x <= startPosition.x - runRange)
            {
                movingLeft = false; // ������ ����������� �� ������
                sprite.flipX = true; // ������������� ������
            }
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;

            // ���������, �������� �� �� ������ �������
            if (transform.position.x >= startPosition.x + runRange)
            {
                movingLeft = true; // ������ ����������� �� �����
                sprite.flipX = false; // ������������� ������
            }
        }
    }
}
