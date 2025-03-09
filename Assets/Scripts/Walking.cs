using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walking : MonoBehaviour
{
    private NavMeshAgent agent; // ���� ������ (������ � ��������� ��� �����������)
    private Animator animator; // �������� ������
    private float timer; // ������� ������� ��������
    private float ChangeTime; // ����� ��� ����� ���������
    private bool IsMoving; // �������� ��������
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // ��������� ���������� ��� �����������
        animator = GetComponent<Animator>(); // ��������� ���������� ��� ���������� ����������
        animator.SetInteger("AnimationID", 0); // �������� ������� (����� 0)
        IsMoving = false;
        ChangeTime = Random.Range(4f, 10f); // ��������� ����� �� ����� ���������

    }
    void Update()
    {
        timer += Time.deltaTime; // �������� �������

        if (timer >= ChangeTime) // ���� ������ ���������
        {
            if (IsMoving) // �������� ��������
            {
                agent.isStopped = true; // ���������
                animator.SetInteger("AnimationID", 7); // �������� ������� (����� 7)
                IsMoving = false;
                //animator.SetInteger("AnimationID", 0); // �������� �������
            }
            else
            {   
                SetRandomDestination(); // ������ ���������
                int randomAnimation = Random.Range(2, 4); // ��������� ��������: 2 - walking01, 2 - walking02
                animator.SetInteger("AnimationID", randomAnimation); // ��������� ��������
                IsMoving = true;
            }

            timer = 0; // ����� ��������
            ChangeTime = Random.Range(5f, 10f); // ��������� ����� �� ��������� ����� ���������
        }

    }
    void SetRandomDestination()
    {
        Vector3 RandomDirection = Random.insideUnitSphere; // ���������� ��������� ����� ������ ����� �������� 1 � ���������� �� 10, ����� ��������� ����������
        RandomDirection += transform.position; // ���������� ������� �������, ����� ����� ���� �� �������� ���������� ������������ ������
        NavMeshHit point; // ���������� ���������� � ����� �� NavMesh
        NavMesh.SamplePosition(RandomDirection, out point, 1, NavMesh.AllAreas); // ���� ��� ����� �� NavMesh ������ ����������� ����� �� ���������� <= 10
        agent.SetDestination(point.position); // ����������� ������-������ ����� ����������
        agent.isStopped = false; // �������� ��������
    }
}
