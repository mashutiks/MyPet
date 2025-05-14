using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dialogue_npc : MonoBehaviour
{
    float hunger; // ������� ����� ��� ������� ������
    float energy; // ������� ������� ��� ������� ������
    float mood; // ������� ��������� ��� ������� ������
    private bool dialogue_started = false; // ���� �� ����� �������
    public GameObject Dialogue; // ������ �� ������ �������
    public GameObject Hello_panel; // ������ �� ������ ��������� �������
    public GameObject Advice_for_dog; // ������ �� ������ � �������� ��� ������ ������
    private string dog_id; // ���������� � ID ������
    private List<int> nodes_for_small_dog = new List<int>(); // ���� ��� ��� ��������� ������
    private List<int> nodes_for_average_dog = new List<int>(); // ���� ��� ��� ������� ������ (�����, ��� ��� ����������)
    private List<int> nodes_for_big_dog = new List<int>(); // ���� ��� ��� ������� ������

    [System.Serializable] // ����� � ���������� ���������
    public class dialogue
    {
        public string npc_text; // ����� ���
        public string answer_variant; // �������� ������ ��� ������ 
        public int next_node; // ��������� ��������� ����
    }

    public dialogue[] nodes; // ������ ���
    public int curr_node; // ������� ���� ��� �������
    public int curr_node_from_advice; // ������� ���� ��� ������
    public TextMeshProUGUI npc_text; // ������ �� ����� ��������
    public TextMeshProUGUI advice_text;
    public Button start_dialogue; // ������ ������, "�������� �����"
    public Button exit_dialogue; // ������ ������ �� ������ ������ ����
    public Button Answer_1; // ������ - ����� ��� �������
    public Button Answer_2; // ����� ����� �� �������
    public Button advice; // ������ �������� ������
    public Button exit_advice; // ������ ������ �� ������ �������

    private enum Action { Eat, Walk, Happy, Idle } // ��������� 
    void Start()
    {
        dog_id = PlayerPrefs.GetString("SelectedDogID", ""); // �������� ������ ������
        List<int> curr_nodes_for_dog = choose_node_for_dog(); // ������ ��� ��� ������� ��� ����������� ������
        curr_node_from_advice = curr_nodes_for_dog[0]; // ������ ���� - � ���� ���������
        //Debug.Log(dog_id);
        Debug.Log(curr_node_from_advice);
        hunger = PlayerPrefs.GetFloat("Hunger", 0f); // ����� �������� �������
        energy = PlayerPrefs.GetFloat("Walk", 0f); // ����� �������� ������� ��������
        mood = PlayerPrefs.GetFloat("Happiness", 0f); // ����� �������� �������

        //foreach (int num in curr_nodes_for_dog)
        //{
        //    Debug.Log(num);
        //}
        //Debug.Log($"�����: {hunger}");
        //Debug.Log($"�����: {energy}");
        //Debug.Log($"�������: {mood}");
        Hello_panel.SetActive(true); // ��������� ������ �����������
        Dialogue.SetActive(false); // �������� ��� ������ ���������� ������
        Advice_for_dog.SetActive(false); // �������� ������ ��� ������
        //Button start_button = Hello_panel.transform.Find("start_dialogue").GetComponent<Button>();
        //Button exit_button = Hello_panel.transform.Find("exit_dialogue").GetComponent<Button>();
        start_dialogue.onClick.AddListener(start_dialogue_function); // ������ ������ ������� ��������� ������� ������ �������
        exit_dialogue.onClick.AddListener(exit_hello_function); // ������ ������ �� �������������� ������ ��������� ������� �������� ������

        Answer_1.onClick.AddListener(start_advice_function); // ����� ������� ������ ����� ������
        Answer_2.onClick.AddListener(exit_dialogue_function); // ������ ������ �� ���������� ������ ��������� ������� �������� ������

        advice.onClick.AddListener(select_answer_from_advice); // ������ �������� �� ���������� ������� (����� ��� ����� �� �������)
        exit_advice.onClick.AddListener(exit_advice_function); // ������ ������ �� ������ ������� ��������� ������� �������� ������

        //show_dialogue();
    }

    void start_dialogue_function()
    {
        Hello_panel.SetActive(false); // ����������� �������������� ������
        Dialogue.SetActive(true); // ���������� ���������� ������
        //Advice_for_dog.SetActive(false); // ����������� 
        int utility_ai_node = choose_best_action(); // ���� ���� ������� ����������� � ������� utility ai � ����������� �� ����
                                                    // ��� ����� ������
        curr_node = utility_ai_node; // ��� ���������� �������
        //show_dialogue(); 
        npc_text.text = nodes[curr_node].npc_text; // ����� ���� ��� - ��� ������� ������� 
        Answer_1.GetComponentInChildren<TextMeshProUGUI>().text = nodes[curr_node].answer_variant;
    }

    void start_advice_function()
    {
        Dialogue.SetActive(false);
        Advice_for_dog.SetActive(true);
        int utility_ai_node = choose_best_action();
        int node_for_dog;
        if (utility_ai_node == 1)
        {
            node_for_dog = choose_node_for_dog()[0];
        }
        else if (utility_ai_node == 2)
        {
            node_for_dog = choose_node_for_dog()[1];
        }
        else
        {
            node_for_dog = choose_node_for_dog()[2];
        }
        //int node_for_dog = choose_node_for_dog()[0];
        curr_node = node_for_dog;
        advice_text.text = nodes[curr_node].npc_text; // ����� ���� ��� - ��� ������� ������� 
        advice.GetComponentInChildren<TextMeshProUGUI>().text = nodes[curr_node].answer_variant;
    }

    void exit_dialogue_function()
    {
        Dialogue.SetActive(false); // �������� ����������� ����
    }

    void exit_advice_function()
    {
        Advice_for_dog.SetActive(false); // �������� ���� �������
    }

    void exit_hello_function()
    {
        Hello_panel.SetActive(false); // �������� ��������������� ����
    }

    void select_answer()
    {
        if (nodes[curr_node].next_node == -1 || nodes[curr_node].next_node == 0) // ���� ���� = -1 ��� 0, �� ������ ����������� 
        {
            exit_dialogue_function();
            return;
        }
        curr_node = nodes[curr_node].next_node; // ��������� ����
        show_dialogue();
    }

    void select_answer_from_advice() // ��� �������
    {
        if (nodes[curr_node].next_node == -1 || nodes[curr_node].next_node == 0)
        {
            exit_dialogue_function();
            exit_advice_function();
            return;
        }
        curr_node = nodes[curr_node_from_advice].next_node;
        show_dialogue();
    }

    void show_dialogue()
    {
        npc_text.text = nodes[curr_node].npc_text; // ����� ���� ��� - ��� ������� ������� 
        advice.GetComponentInChildren<TextMeshProUGUI>().text = nodes[curr_node].answer_variant; // ���������� ������ ������, �������� �� ������ ����
    }

    List<int> choose_node_for_dog()
    {
        dog_id = PlayerPrefs.GetString("SelectedDogID", ""); // �����, ����� � ��� ������
        if (dog_id == "chihuahua") // ��������� ���� ��� ��� �����
        {
            nodes_for_small_dog.Add(5);
            nodes_for_small_dog.Add(6);
            nodes_for_small_dog.Add(7);
            return nodes_for_small_dog;
            //return next_node;
        }
        else if (dog_id == "cur" || dog_id == "corgi" || dog_id == "pug") // ��������� ���� ��� ��� ������� �����
        {
            nodes_for_average_dog.Add(8);
            nodes_for_average_dog.Add(6);
            nodes_for_average_dog.Add(10);
            return nodes_for_average_dog;
        }
        else // ��������� ���� ��� ��� ������� ������
        {
            nodes_for_big_dog.Add(9);
            nodes_for_big_dog.Add(6);
            nodes_for_big_dog.Add(11);
            return nodes_for_big_dog;
        }
    }

    int choose_best_action()
    {
        float hunger_score = hunger / 100f; // �������� ������
        float energy_score = energy / 100f; // �������� �������� (������,��� ������)
        float mood_score = mood / 100f; // �������� ������������
        //float priority_1;
        //float priority_2;
        //float priority_3; // ����� ������� ����� ������ �������, ���� �� ���������� ������ ����� (�����������)
        if (hunger_score <= 0.4f)
        {
            return 1;
        }
        else if (energy_score <= hunger_score && energy_score <= mood_score && energy_score <= 0.4f)
        {
            return 2;
        }
        else if (mood_score < energy_score && mood_score < hunger_score && mood_score <= 0.4f)
        {
            return 3;
        }
        else if (hunger_score > 0.4f && energy_score > 0.4f && mood_score > 0.4f)
        {
            return 4;
        }
        return -1;
    }
}