using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // скорость движения
    [SerializeField] private float jump_force = 7f; // сила прыжка
    private bool is_grounded = false; // на земле ли объект

    private Rigidbody2D rigid_body; // ссылка на компонент
    private Animator animations; // ссылка на компонент анимации
    private SpriteRenderer sprite; // ссылка на компонент где изображение собаки
    private AudioSource audioSource; // ссылка на компонент AudioSource

    private States State
    {
        get { return (States)animations.GetInteger("state"); } // получение значений из аниматора
        set { animations.SetInteger("state", (int)value); } // изменение значений
    }

    private void Awake()
    {
        rigid_body = GetComponent<Rigidbody2D>(); // получение компонента
        animations = GetComponent<Animator>(); // получение компонента
        sprite = GetComponentInChildren<SpriteRenderer>(); // получение компонента из дочернего объекта
        audioSource = GetComponent<AudioSource>(); // получение компонента AudioSource
    }

    void Start()
    {
    }

    private void FixedUpdate()
    {
        CheckGrounded();
    }
    void Update()
    {
        if (is_grounded)
        {
            State = States.idle; // состояние покоя
        }

        if (Input.GetButton("Horizontal"))
        {
            Run();
        }
        if (is_grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) // проверка на нажатие пробела или стрелки вверх
            {
                Jump();
            }
        }
    }

    private void Run()
    {
        if (is_grounded) State = States.run; // состояние бега
        Vector3 direction = transform.right * Input.GetAxis("Horizontal"); // перемещение по горизонтали
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        // текущее местоположение, место перемещения, скорость
        sprite.flipX = direction.x < 0.0f; // если направление < 0, то поворот влево
    }

    private void Jump()
    {
        rigid_body.AddForce(transform.up * jump_force, ForceMode2D.Impulse);
    }

    private void CheckGrounded()
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.4f, 0.3f), 0f); // массив коллайдеров
        is_grounded = collider.Length > 1; // если есть коллайдер под ногами, то мы на земле
        if (!is_grounded) State = States.jump; // если не стоим на земле - прыгаем
    }

    // Новый метод для обработки столкновений
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            if (audioSource != null)
            {
                audioSource.Stop(); // Останавливаем предыдущий звук (если нужно)
                audioSource.Play(); // Проигрываем звук снова
            }
            else
            {
                Debug.Log("AudioSource не найден.");
            }
        }
    }


}

public enum States // перечисление всех видов анимации
{
    idle,
    run,
    jump
}
