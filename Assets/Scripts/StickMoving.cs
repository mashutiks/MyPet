using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMoving : MonoBehaviour
{
    //private bool is_moving_mouse = false; // ���� ���������� �� ������������ ����
    //public LineController line_controller; // �������� ������, ���������� �� ��������� ����������
    //public float power = 10f; // ���� ������
    //private Vector3 start_mouse_position; // ��������� ������� ����
    //void OnMouseDown() // ���� ������
    //{
    //    is_moving_mouse = true; // ���� ������
    //    start_mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ���������� ��������� ������� ����
    //    line_controller.ShowLine();
    //}

    //void OnMouseUp() // ��������� ����
    //{
    //    is_moving_mouse = false; // ���� ��������
    //    line_controller.HideLine(); // �������� �����
    //}
    //void OnMouseDrag() // ���� ������
    //{
    //    if (is_moving_mouse)
    //    {
    //        float enter;
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        new Plane(-Vector3.forward, transform.position).Raycast(ray, out enter);
    //        Vector3 mouse_world_position = ray.GetPoint(enter); // ������� ���� � ������� �����������
    //        Vector3 speed = (mouse_world_position - transform.position) * power; // ������������ �������� ����� �����
    //        line_controller.DrawLine(transform.position, speed); // ������ ����������
    //    }
    //}

}
