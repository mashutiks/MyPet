using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed = 5f; // Скорость движения птицы

    void Update()
    {
        // Двигаем птицу в заданном направлении
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Удаляем птицу, если она выходит за пределы экрана
        if (transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
