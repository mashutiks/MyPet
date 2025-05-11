using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMoving : MonoBehaviour
{
    //private bool is_moving_mouse = false; // флаг отвечающий за передвижение мыши
    //public LineController line_controller; // получаем скрипт, отвечающий за рисование траектории
    //public float power = 10f; // сила броска
    //private Vector3 start_mouse_position; // начальная позиция мыши
    //void OnMouseDown() // мышь зажата
    //{
    //    is_moving_mouse = true; // мышь зажата
    //    start_mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // запоминаем начальную позицию мыши
    //    line_controller.ShowLine();
    //}

    //void OnMouseUp() // отпустили мышь
    //{
    //    is_moving_mouse = false; // мышь отпущена
    //    line_controller.HideLine(); // скрываем линию
    //}
    //void OnMouseDrag() // мышь зажата
    //{
    //    if (is_moving_mouse)
    //    {
    //        float enter;
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        new Plane(-Vector3.forward, transform.position).Raycast(ray, out enter);
    //        Vector3 mouse_world_position = ray.GetPoint(enter); // позиция мыши в мировых координатах
    //        Vector3 speed = (mouse_world_position - transform.position) * power; // рассчитываем скорость полёта палки
    //        line_controller.DrawLine(transform.position, speed); // рисуем траекторию
    //    }
    //}

}
