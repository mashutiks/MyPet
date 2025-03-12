using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Walking : MonoBehaviour
{
    private NavMeshAgent agent; // ���� ������ (������ � ��������� ��� �����������)
    private Animator animator; // �������� ������
    public float WalkingSpeed = 0.5f; // �������� ������ ������ (����� ������ � ���������� � ������� ������� �������)
    public float RunningSpeed = 1f; // �������� ���� ������ (����� ������ � ���������� � ������� ������� �������)

    private float timer; // ������� ������� ��������
    private float ChangeTime; // ����� ��� ����� ���������
    private bool IsMoving; // �������� ��������
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // ��������� ���������� ��� �����������
        animator = GetComponent<Animator>(); // ��������� ���������� ��� ���������� ����������

        animator.SetInteger("AnimationID", 0); // �������� ������� (����� 0)
        IsMoving = false;
        ChangeTime = 2f; // ������������ ��������� ��������

    }
    void Update()
    {
        timer += Time.deltaTime; // �������� ������� �������
        if (timer >= ChangeTime)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); // �������� ��������� ��������
            if ((state.IsName("SittingStart") || state.IsName("SittingCycle")) && state.normalizedTime >= 1.0f) // ���� ������ ����� � ������ ������
            {
                animator.SetInteger("AnimationID", 0); // ������
            }
            if (!IsMoving) // ���� ������ �� ���������
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
            }
            else if (IsMoving) // ���� ������ ���������
            {
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    int RandomAnimation = Random.Range(0, 2); // 0 - breathing, 2 - sitting (��� ���������� ������ ������ �� ����, �� ����� ���� 7 - sitting)
                    if (RandomAnimation == 0) // �������
                    {
                        animator.SetInteger("AnimationID", 0);
                    }
                    else // �������
                    {
                        animator.SetInteger("AnimationID", 7);
                    }
                    IsMoving = false;
                }
            }
            timer = 0; // ����� ��������
        }

    }
}
