using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // переменна€ дл€ направлени€ движени€; true Ч движение вправо
    private bool moveRight = true;
    // скорость движени€ платформы
    private float speed = 2f;
    // лева€ граница движени€ платформы
    private float leftBoundary = 19.10f;
    // права€ граница движени€ платформы
    private float rightBoundary = 20.10f;

    void Update()
    {
        // проверка, достигла ли платформа правой границы, чтобы сменить направление на "влево"
        if (transform.position.x >= rightBoundary)
        {
            moveRight = false;
        }
        // проверка, достигла ли платформа левой границы, чтобы сменить направление на "вправо"
        else if (transform.position.x <= leftBoundary)
        {
            moveRight = true;
        }

        // установка направлени€ движени€: 1, если вправо; -1, если влево
        float direction = moveRight ? 1 : -1;
        // изменение позиции платформы в зависимости от направлени€, скорости и времени между кадрами
        transform.position = new Vector2(transform.position.x + direction * speed * Time.deltaTime, transform.position.y);
    }

    // метод вызываетс€, когда объект находитс€ в контакте с платформой
    private void OnCollisionStay2D(Collision2D collision)
    {
        // провер€ем, если объект имеет тег "Player" (например, персонаж)
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // установка родительского объекта дл€ игрока, чтобы он двигалс€ вместе с платформой
        }
    }

    // метод вызываетс€, когда объект покидает контакт с платформой
    private void OnCollisionExit2D(Collision2D collision)
    {
        // проверка, если объект имеет тег "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // сн€тие родительского объекта, чтобы игрок больше не двигалс€ вместе с платформой
        }
    }
}
