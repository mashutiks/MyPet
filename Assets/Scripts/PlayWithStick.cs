using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//public class PlayWithStick : MonoBehaviour
//{
//    private NavMeshAgent agent; // наша собака (точнее её компонент для перемещения)
//    private Animator animator; // анимации собаки

//    public Transform stick; // ссылка на объект палки
//    public LineController line_controller; // подключаем скрипт управления броском, чтобы контролировать, упала ли палка

//    private int dog_state = 0;
//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>(); // получение компонента для перемещения
//        animator = GetComponent<Animator>(); // получение компонента для управления анимациями
//    }
//    void Update()
//    {
//        switch (dog_state)
//        {
//            case 0:
//                if (line_controller.is_stick_fall)
//                {
//                    Vector3 stop_position = new Vector3(stick.position.x - 1, stick.position.y, stick.position.z - 1);
//                    Vector3 direction = (stop_position - transform.position).normalized; // поворот в направлении палки
//                    Quaternion lookRotation = Quaternion.LookRotation(direction); // поворот в направлении палки
//                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
//                    animator.SetInteger("AnimationID", 4); // включаем бег
//                    agent.SetDestination(stop_position); // до палкки
//                }
//                dog_state = 1;
//                break;
//            case 1:
//                animator.SetInteger("AnimationID", 0);
//                dog_state = 0;
//                break;
//        }


//    }
//}

public class PlayWithStick : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public Transform stick;
    public LineController line_controller;

    private enum DogState { Idle, Running, Rotating, Sitting }
    private DogState currentState = DogState.Idle;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = stoppingDistance;
        agent.updateRotation = false; // Отключаем встроенный поворот
    }

    void Update()
    {
        switch (currentState)
        {
            case DogState.Idle:
                if (line_controller.is_stick_fall)
                {
                    StartRunning();
                }
                break;

            case DogState.Running:
                UpdateRunning();
                break;

            case DogState.Rotating:
                UpdateRotating();
                break;

            case DogState.Sitting:
                // Ничего не делаем, собака сидит
                break;
        }
    }

    void StartRunning()
    {
        // Точка остановки в 1 метре от палки
        Vector3 toStick = stick.position - transform.position;
        targetPosition = stick.position - toStick.normalized * stoppingDistance;

        agent.SetDestination(targetPosition);
        animator.SetInteger("AnimationID", 4); // Анимация бега
        currentState = DogState.Running;
    }

    void UpdateRunning()
    {
        // Плавный поворот во время бега
        if (agent.velocity != Vector3.zero)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                Time.deltaTime * rotationSpeed
            );
        }

        // Проверка достижения точки остановки
        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            StartRotatingToStick();
        }
    }

    void StartRotatingToStick()
    {
        agent.isStopped = true;
        Vector3 toStick = stick.position - transform.position;
        targetRotation = Quaternion.LookRotation(toStick.normalized);
        currentState = DogState.Rotating;
    }

    void UpdateRotating()
    {
        // Плавный поворот к палке
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );

        // Если поворот почти завершен
        if (Quaternion.Angle(transform.rotation, targetRotation) < 2f)
        {
            SitDown();
        }
    }

    void SitDown()
    {
        transform.rotation = targetRotation; // Точный поворот
        animator.SetInteger("AnimationID", 0); // Анимация сидения
        currentState = DogState.Sitting;
    }

    // Для сброса состояния при новом броске
    public void ResetToIdle()
    {
        currentState = DogState.Idle;
        agent.isStopped = false;
    }
}
