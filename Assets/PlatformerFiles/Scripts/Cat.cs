using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public float speed = 2f; // Скорость движения кота
    public float runRange = 3f; // Диапазон бега (влево и вправо)
    private Vector3 startPosition; // Начальная позиция
    private bool movingLeft = true; // Направление движения

    private SpriteRenderer sprite; // Компонент SpriteRenderer

    void Start()
    {
        startPosition = transform.position; // Сохраняем начальную позицию
        sprite = GetComponentInChildren<SpriteRenderer>(); // Получаем компонент SpriteRenderer
    }

    void Update()
    {
        MoveCat();
    }

    private void MoveCat()
    {
        // Определяем направление движения
        if (movingLeft)
        {
            transform.position -= Vector3.right * speed * Time.deltaTime;

            // Проверяем, достигли ли мы левой границы
            if (transform.position.x <= startPosition.x - runRange)
            {
                movingLeft = false; // Меняем направление на вправо
                sprite.flipX = true; // Разворачиваем спрайт
            }
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;

            // Проверяем, достигли ли мы правой границы
            if (transform.position.x >= startPosition.x + runRange)
            {
                movingLeft = true; // Меняем направление на влево
                sprite.flipX = false; // Разворачиваем спрайт
            }
        }
    }
}
