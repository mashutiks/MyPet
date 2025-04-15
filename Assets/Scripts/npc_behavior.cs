using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class npc_behavior : MonoBehaviour
{
    private NavMeshAgent Humanoid;
    private Animator animator;
    public float WalkingSpeed = 0.5f;
    public float RunningSpeed = 1f;

    private float timer;
    private float ChangeTime;
    private bool IsMoving;
    private enum dog_state { Idle, Walk, Breath, Angry, Wiggle_tail, Run, Sit }
    private dog_state curState = dog_state.Idle; // for holding stay = 0/walk = 1 state. for more states using enum may be more comfortable
    void Start()
    {
        Humanoid = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        animator.SetInteger("AnimationID", 0); // дышит
        IsMoving = false;
        ChangeTime = 2f;
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
                            animator.SetInteger("AnimationID", 0);
                            curState = dog_state.Breath;
                            break;
                        case 1:
                            animator.SetInteger("AnimationID", 7);
                            curState = dog_state.Sit;
                            break;
                        case 2:
                            animator.SetInteger("AnimationID", 1);
                            curState = dog_state.Wiggle_tail;
                            break;
                    }

                    //if (RandomAnimation == 0) 
                    //{
                    //    animator.SetInteger("AnimationID", 0);
                    //}
                    //else 
                    //{
                    //    animator.SetInteger("AnimationID", 7);
                    //}
                    IsMoving = false;
                    timer = 0;
                }
                break;

            case dog_state.Idle: //idle
                timer += Time.deltaTime;
                if (timer >= ChangeTime)
                {
                    animator.SetInteger("AnimationID", -1);
                    timer = 2f;
                    curState = dog_state.Breath;
                }
                break;

            case dog_state.Breath:
                AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
                if (state.IsName("None"))
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










