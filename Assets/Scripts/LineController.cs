using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer; // ��������� �� ����������, ���������� �� ��������� �����

    public Transform stick; // ������ �� ������ �����
    public Rigidbody stick_rb; // ������ �� ��������� RigidBody � �����

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
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>(); // �������� ��������� �� ����������
        stick_rb = stick.GetComponent<Rigidbody>(); // �������� ��������� �����, ���������� �� ������
        stick_rb.isKinematic = true; // ��������� ������
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
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && !is_flying) // ������ �� ����� ����� ������� ���� � �� �� � ��������� �����
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ������ ��� �� ��� �����, ��� ��������
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == stick) // ���������, ��� ������ �� ������� ����� ��������� ����� �����
            {
                is_mouse_moving = true;
                lineRenderer.enabled = true; // ��������� ���������� �� ������
                stick_rb.isKinematic = true; // ��������� �����
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
        Vector3 midPoint = (start + end) * 0.5f; // ����� ������� ����� � ��������
        midPoint.y += trajectory_height;

        return Vector3.Lerp(
            Vector3.Lerp(start, midPoint, time), // ����� �������� �� ������ �� ��������
            Vector3.Lerp(midPoint, end, time), // ����� �������� �� �������� �� ������� �����
            time
        );
    }

    void StartFlying()
    {
        is_flying = true;
        flight_start_time = Time.time;
        flight_start_position = stick.position;
        stick_rb.isKinematic = false; // �������� ������

        // ��������� �������� ��� ��������
        Vector3 end_point = points[points.Length - 1]; // ����� ����������� ��������� � ������� ����� ���������� ����������
        float distance = Vector3.Distance(flight_start_position, end_point);
        float speed = distance / flying_time; // V = S/t

        Vector3 direction = (end_point - flight_start_position).normalized;
        float angle = Mathf.Atan2(trajectory_height, distance / 2f) * Mathf.Rad2Deg;

        stick_rb.velocity = CalculateSpeed(flight_start_position, end_point, trajectory_height);
    }

    Vector3 CalculateSpeed(Vector3 start, Vector3 end, float height)
    {
        float gravity = Physics.gravity.y;
        float displacementY = end.y - start.y;
        Vector3 displacementXZ = new Vector3(end.x - start.x, 0, end.z - start.z);

        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + velocityY * -Mathf.Sign(gravity);
    }

    void UpdateFlight()
    {
        float elapsedTime = Time.time - flight_start_time;
        float t = elapsedTime / flying_time;

        if (t >= 1f)
        {
            EndFlight();
            return;
        }

        // ������� ����� �� ����������� ��������
        if (stick_rb.velocity != Vector3.zero)
        {
            stick.rotation = Quaternion.LookRotation(stick_rb.velocity);
        }
    }

    void EndFlight()
    {
        is_flying = false;
    }
    //public void DrawLine(Vector3 start_point, Vector3 speed) // �������� ����������
    //{
    //    Vector3[] points = new Vector3[100]; // ������ ����� ��� ��������� ����� (�� ������� ������ ����� ��������� �����)
    //    lineRenderer.positionCount = points.Length; // ����� ���������� ����� ���������� �� ����������

    //    for (int i = 0; i < points.Length; i++)
    //    {
    //        float time = i * 0.1f; // � ������� �� 0 �� 10 ������ ����� ������� ����� ����������� ������ ����� (1-� � 0 �������, 2-� � 0.1 � �.�)
    //        points[i] = start_point + speed * time + Physics.gravity * time * time / 2f;// ������� ��������� ������ �����: x = x0 + V0x*t + g*t^2/2
    //        if (points[i].y < 0)
    //        {
    //            points[i].y = 0;
    //            lineRenderer.positionCount = i + 1;
    //            break;
    //        }
    //    }
    //    lineRenderer.SetPositions(points); // ����������� ����� ���������� �� ����������
    //}
    //public void HideLine() // ������ �����
    //{
    //    lineRenderer.enabled = false;
    //}

    //public void ShowLine() // �������� �����
    //{
    //    lineRenderer.enabled = true;
    //}
}
