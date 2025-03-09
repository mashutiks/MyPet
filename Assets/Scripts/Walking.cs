using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walking : MonoBehaviour
{
    private NavMeshAgent agent; // наша собака (точнее её компонент для перемещения)
    private Animator animator; // анимации собаки
    private float timer; // счётчик времени анимаций
    private float ChangeTime; // время для смены состояния
    private bool IsMoving; // параметр движения
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // получение компонента для перемещения
        animator = GetComponent<Animator>(); // получение компонента для управления анимациями
        animator.SetInteger("AnimationID", 0); // анимация дыхания (номер 0)
        IsMoving = false;
        ChangeTime = Random.Range(4f, 10f); // случайное время до смены состояния

    }
    void Update()
    {
        timer += Time.deltaTime; // повышаем счётчик

        if (timer >= ChangeTime) // пора менять состояние
        {
            if (IsMoving) // проверка движения
            {
                agent.isStopped = true; // остановка
                animator.SetInteger("AnimationID", 7); // анимация сидения (номер 7)
                IsMoving = false;
                //animator.SetInteger("AnimationID", 0); // анимация дыхания
            }
            else
            {   
                SetRandomDestination(); // собака двигается
                int randomAnimation = Random.Range(2, 4); // случайная анимация: 2 - walking01, 2 - walking02
                animator.SetInteger("AnimationID", randomAnimation); // установка анимации
                IsMoving = true;
            }

            timer = 0; // сброс счётчика
            ChangeTime = Random.Range(5f, 10f); // случайное время до следующей смены состояния
        }

    }
    void SetRandomDestination()
    {
        Vector3 RandomDirection = Random.insideUnitSphere; // выбирается случайная точка внутри сферы радиусом 1 и умножается на 10, чтобы увеличить расстояние
        RandomDirection += transform.position; // прибавляем текущую позицию, чтобы точка была на заданном расстоянии относительно собаки
        NavMeshHit point; // сохранение информации о точке на NavMesh
        NavMesh.SamplePosition(RandomDirection, out point, 1, NavMesh.AllAreas); // ищем эту точку на NavMesh вокруг вычисленной точки на расстоянии <= 10
        agent.SetDestination(point.position); // присваиваем собаке-агенту новую координату
        agent.isStopped = false; // начинаем движение
    }
}
