using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class npc_behavior : MonoBehaviour
{
    private NavMeshAgent Humanoid; // компонент нпс для перемещения 
    private Animator animator; // анимации нпс собаки
    public Transform agent; // позиция игрового пса
    public float WalkingSpeed = 0.5f; // скорость ходьбы
    public float RunningSpeed = 1f; // скорость бега
     
    private float timer; // счётчик времени для анимаций нпс
    private float ChangeTime; // в какой момент мы будем менять анимацию
    private bool IsMoving; // флаг двигается ли
    private enum dog_state { Idle, Walk, Breath, Angry, Wiggle_tail, Run, Sit } // состояния
    private dog_state curState = dog_state.Idle; // for holding stay = 0/walk = 1 state. for more states using enum may be more comfortable
    void Start()
    {
        Humanoid = GetComponent<NavMeshAgent>(); // получение компонента для перемещения
        animator = GetComponent<Animator>(); // получения компонентя для управления анимайиями

        animator.SetInteger("AnimationID", 0); // дышит
        IsMoving = false; // изначально нпс не двиается
        ChangeTime = 2f; // установка времения для смены анимаций - 2 секунды
    }
    void Update()
    {
        switch (curState)
        {
            case dog_state.Walk: //walking
                if (!Humanoid.pathPending && Humanoid.remainingDistance <= Humanoid.stoppingDistance)
                {
                    int RandomAnimation = Random.Range(0, 3); // 0 - breathing, 2 - sitting (��� ���������� ������ ������ �� ����, �� ����� ���� 7 - sitting)
                    switch (RandomAnimation)
                    {
                        case 0:
                            animator.SetInteger("AnimationID", 0); // по рандому выбирает анимацию дыхания
                            curState = dog_state.Breath; // переходим в состояние дыхания
                            break;
                        case 1:
                            animator.SetInteger("AnimationID", 7); // по рандому может выбрать анимацию сидения
                            curState = dog_state.Sit; // выбирается состояние сидения
                            break;
                        case 2:
                            animator.SetInteger("AnimationID", 1); // по рандому выбирает вилянием хвостом
                            curState = dog_state.Wiggle_tail; // переходим в состояние виляния хвостом
                            break;
                    }
                    IsMoving = false; // перестаёт двигаться
                    timer = 0; // таймер обнуляется
                }
                break;

            case dog_state.Idle: //idle
                timer += Time.deltaTime; // считывается время для состояния покоя
                if (timer >= ChangeTime)
                {
                    animator.SetInteger("AnimationID", -1); // переход в None
                    timer = 2f; 
                    curState = dog_state.Breath; // обратно в дыхание
                }
                break;

            case dog_state.Breath:
                AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); // просмотр состояния анимаций
                if (state.IsName("None")) // их none можем переходить далее
                {
                    int RandomAnimation = Random.Range(2, 6); // 2 - walking01, 3 - walking02, 4 - running, 6 - angry
                    if (RandomAnimation == 4)
                    {
                        Humanoid.speed = RunningSpeed;
                    }
                    else
                    {
                        Humanoid.speed = WalkingSpeed;
                    }
                    if (RandomAnimation == 5)
                    {
                        animator.SetInteger("AnimationID", 6); // злой
                        curState = dog_state.Angry;
                        timer = 0;
                    }
                    else
                    {
                        // реализация поворота
                        Vector3 RandomDirection = Random.insideUnitSphere * 3f + transform.position;
                        NavMeshHit point;
                        NavMesh.SamplePosition(RandomDirection, out point, 3f, NavMesh.AllAreas);
                        Vector3 direction = (point.position - transform.position).normalized;
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
                        animator.SetInteger("AnimationID", RandomAnimation);
                        Humanoid.SetDestination(point.position);
                        curState = dog_state.Walk;
                        timer = 0;
                    }
                }
                break;

            case dog_state.Angry:
                timer += Time.deltaTime;
                if (timer > 3f) // если позлилась 3 секунды
                {
                    animator.SetInteger("AnimationID", 0); // снова дыхание
                    curState = dog_state.Idle;
                    timer = 0;
                }
                break;

            case dog_state.Wiggle_tail:
                timer += Time.deltaTime;
                if (timer > 2f) // повиляла хвостиком 2 секунды
                {
                    animator.SetInteger("AnimationID", 0); // после виляния — idle
                    curState = dog_state.Idle;
                    timer = 0;
                }
                break;

            case dog_state.Sit:
                timer += Time.deltaTime;
                if (timer > 5f) // дольше 5 сек сидит
                {
                    animator.SetInteger("AnimationID", 0);
                    curState = dog_state.Idle;
                    timer = 0;
                }
                break;

        }
    }
}










