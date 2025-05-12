using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer; // компонент из инспектора, отвечающий за рисование линии

    public Transform stick; // ссылка на объект палки

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

    public bool is_stick_fall = false; // флаг упала ли палка
    public Button PlayingButton; // кнопка "Играть"
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>(); // получаем компонент из инспектора
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

    void HandleInput() // функци-обработчик нажатия на мышь
    {
        if (Input.GetMouseButtonDown(0) && !is_flying) // нажали на палку левой кнопкой мыши и мы не в состоянии полёта
        {
            is_stick_fall = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // создаём луч из той точки, где кликнули
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == stick) // проверяем, что именно на объекте палки оказалась точка клика
            {
                is_mouse_moving = true;
                lineRenderer.enabled = true; // проявляем траекторию на экране
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
        Vector3 mid_point = (start + end) * 0.5f; // коодината по x самой высокой точки у параболы
        mid_point.y += trajectory_height; // координата по y самой высокой точки у параболы

        return Vector3.Lerp(
            Vector3.Lerp(start, mid_point, time), // ветвь параболы от старта до середины
            Vector3.Lerp(mid_point, end, time), // ветвь параболы от середины до целевой точки
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
        float elapsed_time = Time.time - flight_start_time; // сколько времени прошло от начала полёта
        float t = elapsed_time / flying_time; // чтобы понять, когда заканчивать движение

        if (t >= 1f)
        {
            EndFlight();
            return;
        }
        
        Vector3 new_position = GetPointOnTrajectory(t); // повторяем начерченную траекторию, каждый раз, изменяя текущую позицию
        stick.position = new_position;

      
        if (t > 0.01f && t < 0.99f) // чтобы не было рывков в начале/конце
        {
            Vector3 next_point = GetPointOnTrajectory(t + 0.01f);
            Vector3 move_direction = (next_point - new_position).normalized;

            if (move_direction != Vector3.zero)
                stick.rotation = Quaternion.LookRotation(move_direction); // поворот палки по направлению движения
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
        // сбрасываем все флаги, связанные с броском
        is_stick_fall = false;
        is_flying = false;
        is_mouse_moving = false;

        
        lineRenderer.enabled = false;// отключаем визуализацию траектории

        // очищаем точки траектории
        points = null;
        lineRenderer.positionCount = 0;
        PlayingButton.interactable = true; // разблокируем кнопку
    }

}
