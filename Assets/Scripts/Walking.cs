﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Walking : MonoBehaviour
{
    private NavMeshAgent agent; // наша собака (точнее её компонент для перемещения)
    private Animator animator; // анимации собаки
    private Transform enemy; // позиция нпс
    public float WalkingSpeed = 0.5f; // скорость ходьбы собаки (можно задать в инспекторе в разделе данного скрипта)
    public float RunningSpeed = 1f; // скорость бега собаки (можно задать в инспекторе в разделе данного скрипта)
    public float radius = 1f; // радиус до нпс (пса)

    private float timer; // счётчик времени анимаций
    private float ChangeTime; // время для смены состояния
    private bool IsMoving; // параметр движения
    private int curState = 2; // for holding stay = 0/walk = 1 state. for more states using enum may be more comfortable
    private GameObject npc;
    private int RandomAnimation;
    public AudioSource source;
    public AudioClip bark;

    private bool achievementFriendEarned = false; // Для отслеживания достижения "нашел друга"
    private bool achievementEnemyEarned = false; // Для отслеживания достижения "нашел врага"

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // получение компонента для перемещения
        animator = GetComponent<Animator>(); // получение компонента для управления анимациями
        source = GetComponent<AudioSource>();

        animator.SetInteger("AnimationID", 0); // анимация дыхания (номер 0)
        IsMoving = false;
        ChangeTime = 2f; // длительность состояния анимации
        npc = GameObject.FindGameObjectWithTag("enemy");
        RandomAnimation = Random.Range(10, 12);

    }
    void Update()
    {
        npc = GameObject.FindGameObjectWithTag("enemy");
        //if (npc != null)
        //{
        //    enemy = GameObject.FindGameObjectWithTag("enemy").transform; // сам игровой объект нпс, сохранили в enemy позицию нпс
        //    Vector3 npc_pos = enemy.position;
        //    Debug.Log(npc_pos);
        //    Vector3 dog_pos = transform.position;
        //    Debug.Log(dog_pos);
        //    float distance_dog_npc = (npc_pos - dog_pos).magnitude;
        //    Debug.Log(distance_dog_npc);
        //    if (distance_dog_npc <= 2f)
        //    {
        //        Vector3 direction_to_npc = npc_pos - dog_pos;
        //        Debug.Log(direction_to_npc);
        //        Quaternion look_Rotation = Quaternion.LookRotation(direction_to_npc);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, look_Rotation, Time.deltaTime * 10f);
        //        int RandomAnimation = Random.Range(0, 2);
        //        if (RandomAnimation == 0)
        //        {
        //            animator.SetInteger("AnimationID", 6);
        //        }
        //        else
        //        {
        //            animator.SetInteger("AnimationID", 1);
        //        }
        //    }
        //}

        switch (curState)
        {
            case 2: // agression or wiggling
                if (npc != null)
                {

                    enemy = GameObject.FindGameObjectWithTag("enemy").transform; // сам игровой объект нпс, сохранили в enemy позицию нпс
                    Vector3 npc_pos = enemy.position;
                    //Debug.Log(npc_pos);
                    Vector3 dog_pos = transform.position;
                    //Debug.Log(dog_pos);
                    float distance_dog_npc = (npc_pos - dog_pos).magnitude;
                    //Debug.Log(distance_dog_npc);
                    if (distance_dog_npc <= 2f)
                    {
                        Vector3 direction_to_npc = npc_pos - dog_pos;
                        //Debug.Log(direction_to_npc);
                        Quaternion look_Rotation = Quaternion.LookRotation(direction_to_npc);
                        transform.rotation = Quaternion.Slerp(transform.rotation, look_Rotation, Time.deltaTime * 10f);
                        if (RandomAnimation == 10)
                        {
                            animator.SetInteger("AnimationID", 6);
                            //source.PlayOneShot(bark);
                            if (!source.isPlaying)
                            {
                                source.PlayOneShot(bark);
                            }
                            
                            if (!achievementEnemyEarned)
                            {
                                AchievemenetManager.Instance.EarnAchievement("Лучше 0 врагов");
                                achievementEnemyEarned = true;
                            }
                        }
                        else if (RandomAnimation == 11)
                        {
                            animator.SetInteger("AnimationID", 1);
                            if (!achievementFriendEarned)
                            {
                                AchievemenetManager.Instance.EarnAchievement("Лучше 100 друзей");
                                achievementFriendEarned = true;
                            }
                        }
                    }
                    else
                    {
                        curState = 0;
                    }
                }
                else
                {
                    curState = 0;
                }
                break;
            case 1: //idle
                achievementFriendEarned = false; // Сброс флага
                achievementEnemyEarned = false; // Сброс флага

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    RandomAnimation = Random.Range(0, 2); // 0 - breathing, 2 - sitting (originally: animation 7 - sitting)
                    if (RandomAnimation == 0) // дыхание
                    {
                        animator.SetInteger("AnimationID", 0);
                    }
                    else // сидение
                    {
                        animator.SetInteger("AnimationID", 7);
                    }
                    IsMoving = false;
                    RandomAnimation = Random.Range(10, 12); // подготовка к лаю или вилянию хвостом
                    curState = 2;
                }
                break;
            case 0: //walking
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
                        RandomAnimation = Random.Range(2, 5); // 2 - walking01, 3 - walking02, 4 - running
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