using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer; // ��������� �� ����������, ���������� �� ��������� �����

    public Transform stick; // ������ �� ������ �����

    public LayerMask floor_layer; // ������ �� ��������� ���� ����, ����� ����� ����� ���� ������ ������ �� ���

    public float trajectory_height = 10f; // ������ ��������
    public int count_points = 30; // ���������� �����, �� ������� ����� ��������� ��������
    public float max_distance = 50f; // ������������ ���������� ����� ������ � ������ �� ������

    public float force = 10f; // ���� ������
    public float flying_time = 2f; // ������������ �����
    private bool is_flying = false; // ���� ��� �������� ������� �����
    private float flight_start_time; // ��������� ����� �����
    private Vector3 flight_start_position; // ��������� �����

    private bool is_mouse_moving = false; // ����, ���������� �� ������� ����� ������ ����
    private Vector3 current_floor_point; // ������� ����� �� ���� (���� �� ���, � ������� ����� ������������ �����)
    private Vector3[] points; // �����, �� ������� �������� ��������

    public bool is_stick_fall = false; // ���� ����� �� �����
    public Button PlayingButton; // ������ "������"
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>(); // �������� ��������� �� ����������
        is_stick_fall = false;
    }

    void Update()
    {
        HandleInput();
        if (is_mouse_moving)
        {
            UpdateTrajectory();
        }
        else if (is_flying)
        {
            UpdateFlight();
        }
        Debug.Log(is_stick_fall);
    }

    void HandleInput() // ������-���������� ������� �� ����
    {
        if (Input.GetMouseButtonDown(0) && !is_flying) // ������ �� ����� ����� ������� ���� � �� �� � ��������� �����
        {
            is_stick_fall = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ������ ��� �� ��� �����, ��� ��������
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == stick) // ���������, ��� ������ �� ������� ����� ��������� ����� �����
            {
                is_mouse_moving = true;
                lineRenderer.enabled = true; // ��������� ���������� �� ������
            }
        }

        if (Input.GetMouseButtonUp(0) && is_mouse_moving) // ��������� ����� ������ ����
        {
            is_mouse_moving = false;
            lineRenderer.enabled = false; // �������� ����������
            if (points != null && points.Length > 1)
            {
                StartFlying(); // �����
            }
        }
    }

    void UpdateTrajectory()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ��������� ����� �� ����
        RaycastHit floor_point;

        if (Physics.Raycast(ray, out floor_point, Mathf.Infinity, floor_layer))
        {
            current_floor_point = floor_point.point; // ������� ����� = �������
            current_floor_point.y = 0; // ������ ������� ����������

            Vector3 direction = current_floor_point - stick.position; // ��������� ���������� ����� ������� ������ � ����
            if (direction.magnitude > max_distance) // ����� ������ ���� �� ����� ������ �����
            {
                current_floor_point = stick.position + direction.normalized * max_distance;
            }

            DrawLine(stick.position, current_floor_point); // ������ �������� �� ����� �� ����� �� ����
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        points = new Vector3[count_points]; // ������ ����� ��� ��������� ����� (�� ������� ������ ����� ��������� �����)

        for (int i = 0; i < count_points; i++)
        {
            float time = i / (float)(count_points - 1); // ����� ��� ������������ ������������� �����
            points[i] = SetPoint(start, end, time); // ��������� ����� ��� ��������� ��������
        }

        lineRenderer.positionCount = count_points;
        lineRenderer.SetPositions(points); // ����������� ����� ���������� �� ����������
    }

    Vector3 SetPoint(Vector3 start, Vector3 end, float time)
    {
        Vector3 mid_point = (start + end) * 0.5f; // ��������� �� x ����� ������� ����� � ��������
        mid_point.y += trajectory_height; // ���������� �� y ����� ������� ����� � ��������

        return Vector3.Lerp(
            Vector3.Lerp(start, mid_point, time), // ����� �������� �� ������ �� ��������
            Vector3.Lerp(mid_point, end, time), // ����� �������� �� �������� �� ������� �����
            time
        );
    }

    void StartFlying()
    {
        is_flying = true;
        flight_start_time = Time.time;
        flight_start_position = stick.position;
    }

    void UpdateFlight()
    {
        float elapsed_time = Time.time - flight_start_time; // ������� ������� ������ �� ������ �����
        float t = elapsed_time / flying_time; // ����� ������, ����� ����������� ��������

        if (t >= 1f)
        {
            EndFlight();
            return;
        }
        
        Vector3 new_position = GetPointOnTrajectory(t); // ��������� ����������� ����������, ������ ���, ������� ������� �������
        stick.position = new_position;

      
        if (t > 0.01f && t < 0.99f) // ����� �� ���� ������ � ������/�����
        {
            Vector3 next_point = GetPointOnTrajectory(t + 0.01f);
            Vector3 move_direction = (next_point - new_position).normalized;

            if (move_direction != Vector3.zero)
                stick.rotation = Quaternion.LookRotation(move_direction); // ������� ����� �� ����������� ��������
        }
        
    }
    Vector3 GetPointOnTrajectory(float t)
    {
        Vector3 start = flight_start_position;
        Vector3 end = points[points.Length - 1];
        Debug.Log(end);
        Vector3 mid = (start + end) * 0.5f + Vector3.up * trajectory_height;

        return Vector3.Lerp(Vector3.Lerp(start, mid, t), Vector3.Lerp(mid, end, t), t);
    }
    void EndFlight()
    {
        is_flying = false;
        is_stick_fall = true;
    }

    public void ResetThrow()
    {
        // ���������� ��� �����, ��������� � �������
        is_stick_fall = false;
        is_flying = false;
        is_mouse_moving = false;

        
        lineRenderer.enabled = false;// ��������� ������������ ����������

        // ������� ����� ����������
        points = null;
        lineRenderer.positionCount = 0;
        PlayingButton.interactable = true; // ������������ ������
    }

}
