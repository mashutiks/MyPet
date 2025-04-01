using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    private Vector3 startPosition; //начальная позиция персонажа

    void Start()
    {
        // сохраняем начальную позицию персонажа
        startPosition = transform.position;
    }

    void Update()
    {
        // проверка, упал ли персонаж ниже определенной высоты
        if (transform.position.y < -10) 
        {
            ResetPosition();
        }
    }

    // возвращение персонажа на начальную позицию
    void ResetPosition()
    {
        transform.position = startPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy")) 
        {
            ResetPosition();
        }
    }
}
