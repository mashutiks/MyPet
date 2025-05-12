using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dialogue_npc : MonoBehaviour
{
    float hunger;
    float energy;
    float mood;
    private bool dialogue_started = false;

    private enum Action { Eat, Walk, Happy, Idle } // ��������� 
    void Start()
    {
        hunger = PlayerPrefs.GetFloat("Hunger", 0f);
        energy = PlayerPrefs.GetFloat("Walk", 0f);
        mood = PlayerPrefs.GetFloat("Happiness", 0f);
        //Debug.Log($"�����: {hunger}");
        //Debug.Log($"�����: {energy}");
        //Debug.Log($"�������: {mood}");
    }

    void Update()
    {
        if (!dialogue_started)
        {
            Action best_action = choose_best_action();
            Debug.Log($"{best_action} | �����: {hunger} | �����: {energy} | �������: {mood}");
            StartCoroutine(do_action(best_action));
        }
    }

    private Action choose_best_action()
    {
        float hunger_score = hunger / 100f; // �������� ������
        float energy_score = energy / 100f; // �������� �������� (������,��� ������)
        float mood_score = mood / 100f; // �������� ������������
        //float priority_1;
        //float priority_2;
        //float priority_3; // ����� ������� ����� ������ �������, ���� �� ���������� ������ ����� (�����������)
        if (hunger_score <= 0.4f)
        {
            return Action.Eat;
        }
        else if (energy_score <= hunger_score && energy_score <= mood_score && energy_score <= 0.4f) 
        {
            return Action.Walk;
        }
        else if (mood_score < energy_score && mood_score < hunger_score && mood_score <= 0.4f)
        {
            return Action.Happy;
        }
        else 
        {
            return Action.Idle; // ��� ���������� � �����
        }
    }

    private IEnumerator do_action(Action action)
    {
        dialogue_started = true;
        switch (action)
        {
            case Action.Eat:
                Debug.Log($"������ ��� ��, ��� ����� ������ ������");
                yield return new WaitForSeconds(20f);
                break;

            case Action.Walk:
                Debug.Log($"������ ��� ��, ��� ����� ����������� � ����� ����� ��� �����������");
                yield return new WaitForSeconds(20f);
                break;

            case Action.Happy:
                Debug.Log($"������ ��� ��, ��� ����� ������ �������");
                yield return new WaitForSeconds(20f);
                break;

            case Action.Idle:
                Debug.Log($"��� ������ �������? ��� ����� �����");
                yield return new WaitForSeconds(20f);
                break;
        }
        dialogue_started = false;
    }
}
