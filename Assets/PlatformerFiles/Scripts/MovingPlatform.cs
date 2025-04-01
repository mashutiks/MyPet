using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; // Скорость движения платформы
    public float moveDistance = 5f; // Длина, на которую платформа должна двигаться влево и вправо

    private Vector2 startPosition; // Начальная позиция платформы
    private int direction = 1; // Направление движения (1 = вправо, -1 = влево)
    private Transform player; // Ссылка на игрока
    private bool playerOnPlatform = false; // Проверка, находится ли игрок на платформе

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float distanceFromStart = Vector2.Distance(startPosition, transform.position);

        if (distanceFromStart >= moveDistance)
        {
            direction *= -1;
        }

        // Перемещаем платформу
        Vector2 movement = Vector2.right * direction * speed * Time.deltaTime;
        transform.Translate(movement);

        // Если игрок на платформе, передвигаем его вместе с платформой
        if (playerOnPlatform && player != null)
        {
            // Добавляем движение платформы к позиции игрока
            player.position += new Vector3(movement.x, 0, 0);
        }
    }
    // первый контакт коллайдера объекта с другим объектом
    void OnCollisionEnter2D(Collision2D collision)
    {
        // имеет ли объект, с которым произошёл контакт, тег "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // присваиваю ссылку на объект игрока, чтобы можно было работать с ним
            player = collision.transform;
            // игрок находится на платформе - флажок
            playerOnPlatform = true;
        }
    }
    // объект покидает контакт с другим объектом
    void OnCollisionExit2D(Collision2D collision)
    {
        // объект, с которым завершён контакт, имеет тег "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // игрок больше не на платформе
            playerOnPlatform = false;
            // обнуляется ссылка на объект игррка
            player = null;
        }
    }
}
