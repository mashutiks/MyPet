using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer; // компонент из инспектора, отвечающий за рисование линии
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>(); // получаем компонент из инспектора
    }

   public void DrawLine(Vector3 start_point, Vector3 speed) // рисовать траекторию
   {
        Vector3[] points = new Vector3[100]; // вектор точек для рисования линии (по скольки точкам будет построена линия)
        lineRenderer.positionCount = points.Length; // задаём количество точек компоненту из инспектора

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f; // в течение от 0 до 10 секунд после запуска будет происходить расчёт точки (1-я в 0 секунду, 2-я в 0.1 и т.д)
            points[i] = start_point + speed * time + Physics.gravity * time * time / 2f;// рассчёт положения каждой точки: x = x0 + V0x*t + g*t^2/2
        }
        lineRenderer.SetPositions(points); // присваиваем точки компоненту из инспектора
   }
}
