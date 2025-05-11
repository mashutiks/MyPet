using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer; // компонент из инспектора, отвечающий за рисование линии

    public Transform stick; // ссылка на объект палки
    public Rigidbody stick_rb; // ссылка на компонент RigidBody у палки

    public LayerMask floor_layer; // ссылка на созданный слой пола, чтобы палку можно было кидать только на пол

    public float trajectory_height = 10f; // высота параболы
    public int count_points = 30; // количество точек, из которых будет строиться парабола
    public float max_distance = 50f; // максимальное расстояние между палкой и точкой на газоне

    public float force = 10f; // сила броска
    public float flying_time = 2f; // длительность полёта
    private bool is_flying = false; // флаг для проверки наличия полёта
    private float flight_start_time; // начальное время полёта
    private Vector3 flight_start_position; // начальная точка

    private bool is_mouse_moving = false; // флаг, отвечающий за ведение левой кнопки мыши
    private Vector3 current_floor_point; // текущая точка на полу (одна из тех, в которую может приземлиться палка)
    private Vector3[] points; // точки, из которых строится парабола
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>(); // получаем компонент из инспектора
        stick_rb = stick.GetComponent<Rigidbody>(); // получаем компонент палки, отвечающий за физику
        stick_rb.isKinematic = true; // отключаем физику
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
        if (Input.GetMouseButtonDown(0) && !is_flying) // нажали на палку левой кнопкой мыши и мы не в состоянии полёта
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // создаём луч из той точки, где кликнули
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == stick) // проверяем, что именно на объекте палки оказалась точка клика
            {
                is_mouse_moving = true;
                lineRenderer.enabled = true; // проявляем траекторию на экране
                stick_rb.isKinematic = true; // фиксируем палку
            }
        }

        if (Input.GetMouseButtonUp(0) && is_mouse_moving) // отпустили левую кнопку мыши
        {
            is_mouse_moving = false;
            lineRenderer.enabled = false; // скрываем траекторию
            if (points != null && points.Length > 1)
            {
                StartFlying(); // летим
            }
        }
    }

    void UpdateTrajectory()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // обновляем точку на полу
        RaycastHit floor_point;

        if (Physics.Raycast(ray, out floor_point, Mathf.Infinity, floor_layer))
        {
            current_floor_point = floor_point.point; // текущая точка = целевая
            current_floor_point.y = 0; // придаём нулевую координату

            Vector3 direction = current_floor_point - stick.position; // проверяем расстояние между точками старта и цели
            if (direction.magnitude > max_distance) // чтобы нельзя было за сцену кидать палку
            {
                current_floor_point = stick.position + direction.normalized * max_distance;
            }

            DrawLine(stick.position, current_floor_point); // рисуем параболу от палки до точки на полу
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        points = new Vector3[count_points]; // вектор точек для рисования линии (по скольки точкам будет построена линия)

        for (int i = 0; i < count_points; i++)
        {
            float time = i / (float)(count_points - 1); // время для равномерного распределения точек
            points[i] = SetPoint(start, end, time); // установка точек для рисования параболы
        }

        lineRenderer.positionCount = count_points;
        lineRenderer.SetPositions(points); // присваиваем точки компоненту из инспектора
    }

    Vector3 SetPoint(Vector3 start, Vector3 end, float time)
    {
        Vector3 midPoint = (start + end) * 0.5f; // самая высокая точка у параболы
        midPoint.y += trajectory_height;

        return Vector3.Lerp(
            Vector3.Lerp(start, midPoint, time), // ветвь параболы от старта до середины
            Vector3.Lerp(midPoint, end, time), // ветвь параболы от середины до целевой точки
            time
        );
    }

    void StartFlying()
    {
        is_flying = true;
        flight_start_time = Time.time;
        flight_start_position = stick.position;
        stick_rb.isKinematic = false; // включаем физику

        // Начальная скорость для параболы
        Vector3 end_point = points[points.Length - 1]; // точка приземления последняя в массиве точек построения траектории
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

        // поворот палки по направлению движения
        if (stick_rb.velocity != Vector3.zero)
        {
            stick.rotation = Quaternion.LookRotation(stick_rb.velocity);
        }
    }

    void EndFlight()
    {
        is_flying = false;
    }
    //public void DrawLine(Vector3 start_point, Vector3 speed) // рисовать траекторию
    //{
    //    Vector3[] points = new Vector3[100]; // вектор точек для рисования линии (по скольки точкам будет построена линия)
    //    lineRenderer.positionCount = points.Length; // задаём количество точек компоненту из инспектора

    //    for (int i = 0; i < points.Length; i++)
    //    {
    //        float time = i * 0.1f; // в течение от 0 до 10 секунд после запуска будет происходить расчёт точки (1-я в 0 секунду, 2-я в 0.1 и т.д)
    //        points[i] = start_point + speed * time + Physics.gravity * time * time / 2f;// рассчёт положения каждой точки: x = x0 + V0x*t + g*t^2/2
    //        if (points[i].y < 0)
    //        {
    //            points[i].y = 0;
    //            lineRenderer.positionCount = i + 1;
    //            break;
    //        }
    //    }
    //    lineRenderer.SetPositions(points); // присваиваем точки компоненту из инспектора
    //}
    //public void HideLine() // скрыть линию
    //{
    //    lineRenderer.enabled = false;
    //}

    //public void ShowLine() // показать линию
    //{
    //    lineRenderer.enabled = true;
    //}
}
