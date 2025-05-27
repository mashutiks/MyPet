using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // ссылка на объект игрока
    private Vector3 pos; // вектор для хранения позиции камеры

    private void Awake() // инициализация объекта
    {
        if (!player) // проверяем, назначен ли объект игрока;
                     // если нет, ищем объект типа Dog и берём его Transform
            player = FindObjectOfType<Dog>().transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // позицию камеры на позицию игрока
        pos = player.position;
        // фиксация координаты по z камеры на -10 чтобы камера не сливалась с объектами на сцене
        pos.z = -10f;
        // плавное перемещение камеры к позиции игрока
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}
