using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Walking : MonoBehaviour
{
    private NavMeshAgent agent; // наша собака (точнее её компонент для перемещения)
    private Animator animator; // анимации собаки
    public float WalkingSpeed = 0.5f; // скорость ходьбы собаки (можно задать в инспекторе в разделе данного скрипта)
    public float RunningSpeed = 1f; // скорость бега собаки (можно задать в инспекторе в разделе данного скрипта)

    private float timer; // счётчик времени анимаций
    private float ChangeTime; // время для смены состояния
    private bool IsMoving; // параметр движения
    private int curState = 0; // for holding stay = 0/walk = 1 state. for more states using enum may be more comfortable
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // получение компонента для перемещения
        animator = GetComponent<Animator>(); // получение компонента для управления анимациями

        animator.SetInteger("AnimationID", 0); // анимация дыхания (номер 0)
        IsMoving = false;
        ChangeTime = 2f; // длительность состояния анимации

    }
    void Update()
    {
        switch (curState)
        {
            case 1: //walking
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    int RandomAnimation = Random.Range(0, 2); // 0 - breathing, 2 - sitting (��� ���������� ������ ������ �� ����, �� ����� ���� 7 - sitting)
                    if (RandomAnimation == 0) // дыхание
                    {
                        animator.SetInteger("AnimationID", 0);
                    }
                    else // сидение
                    {
                        animator.SetInteger("AnimationID", 7);
                    }
                    IsMoving = false;
                    curState = 0;
                }
                break;
            case 0: //idle
                timer += Time.deltaTime;
                if (timer >= ChangeTime)
                {
                    AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); // просмотр состояния анимаций
                    if (!state.IsName("None")) // ���� ������ ����� � ������ ������
                    {
                        // changed to -1 so animation can transition to None first
                        animator.SetInteger("AnimationID", -1); // ������
                        timer = 1.5f;
                    }
                    else if (state.IsName("None"))
                    {
                        int RandomAnimation = Random.Range(2, 5); // 2 - walking01, 3 - walking02, 4 - running
                        if (RandomAnimation == 4) // ���� ���
                        {
                            agent.speed = RunningSpeed; // ����������� �������� ��� ����
                        }
                        else
                        {
                            agent.speed = WalkingSpeed; // ����������� �������� ��� ������
                        }
                        Vector3 RandomDirection = Random.insideUnitSphere * 3f; // ����� ��������� ����������� �������� � �������� 5 ������ �� ���������
                        RandomDirection += transform.position; // ���������� ��������� �����, � ������� �������� ��������
                        NavMeshHit point; // ���������� ����� �� �������
                        NavMesh.SamplePosition(RandomDirection, out point, 3f, NavMesh.AllAreas); // ���� ����� ����� �� NavMesh
                        Vector3 direction = (point.position - transform.position).normalized; // ����������� � ����
                        Quaternion lookRotation = Quaternion.LookRotation(direction); // �������������� � ����
                        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f); // ���������� ��������
                        animator.SetInteger("AnimationID", RandomAnimation); // ������ ��������
                        agent.SetDestination(point.position); // ����� ������ ����
                                                              //agent.transform.Translate(Vector3.forward * agent.speed * Time.deltaTime); // �������� ���������
                        IsMoving = true;
                        timer = 0;
                        curState = 1;
                    }
                }

                break;
        }
    }
}





//timer += Time.deltaTime; // повышаем счётчик времени
//if (timer >= ChangeTime)
//{
//    AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); // просмотр состояния анимаций
//    if ((state.IsName("SittingStart") || state.IsName("SittingCycle")) && state.normalizedTime >= 1.0f) // если собака сидит в данный момент
//    {
//        animator.SetInteger("AnimationID", 0); // встать
//    }
//    if (!IsMoving) // если собака не двигалась
//    {
//        int RandomAnimation = Random.Range(2, 5); // 2 - walking01, 3 - walking02, 4 - running
//        if (RandomAnimation == 4) // если бег
//        {
//            agent.speed = RunningSpeed; // присваиваем скорость для бега
//        }
//        else
//        {
//            agent.speed = WalkingSpeed; // присваиваем скорость для ходьбы
//        }
//        Vector3 RandomDirection = Random.insideUnitSphere * 3f; // задаём случайное направление движения в пределах 5 метров от персонажа
//        RandomDirection += transform.position; // координата финальной точки, в которую прибудет персонаж
//        NavMeshHit point; // конкретная точка на локации
//        NavMesh.SamplePosition(RandomDirection, out point, 3f, NavMesh.AllAreas); // ищем такую точку на NavMesh
//        Vector3 direction = (point.position - transform.position).normalized; // направление к цели
//        Quaternion lookRotation = Quaternion.LookRotation(direction); // поворачиваемся к цели
//        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f); // выполнение поворота
//        animator.SetInteger("AnimationID", RandomAnimation); // запуск анимации
//        agent.SetDestination(point.position); // задаём агенту цель
//                                              //agent.transform.Translate(Vector3.forward * agent.speed * Time.deltaTime); // движение персонажа
//        IsMoving = true;
//    }
//    else if (IsMoving) // если собака двигалась
//    {
//        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
//        {
//            int RandomAnimation = Random.Range(0, 2); // 0 - breathing, 2 - sitting (для рандомного выбора одного из двух, на самом деле 7 - sitting)
//            if (RandomAnimation == 0) // дыхание
//            {
//                animator.SetInteger("AnimationID", 0);
//            }
//            else // сидение
//            {
//                animator.SetInteger("AnimationID", 7);
//            }
//            IsMoving = false;
//        }
//    }
//    timer = 0; // сброс счётчика
//}