using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer; // ��������� �� ����������, ���������� �� ��������� �����
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>(); // �������� ��������� �� ����������
    }

   public void DrawLine(Vector3 start_point, Vector3 speed) // �������� ����������
   {
        Vector3[] points = new Vector3[100]; // ������ ����� ��� ��������� ����� (�� ������� ������ ����� ��������� �����)
        lineRenderer.positionCount = points.Length; // ����� ���������� ����� ���������� �� ����������

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f; // � ������� �� 0 �� 10 ������ ����� ������� ����� ����������� ������ ����� (1-� � 0 �������, 2-� � 0.1 � �.�)
            points[i] = start_point + speed * time + Physics.gravity * time * time / 2f;// ������� ��������� ������ �����: x = x0 + V0x*t + g*t^2/2
        }
        lineRenderer.SetPositions(points); // ����������� ����� ���������� �� ����������
   }
}
