using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dialogue_npc : MonoBehaviour
{
    float hunger; // текущий голод для текущей собаки
    float energy; // текущая энергия для текущей собаки
    float mood; // текущее состояние для текущей собаки
    private bool dialogue_started = false; // флаг на старт диалога
    public GameObject Dialogue; // ссылка на панель диалога
    public GameObject Hello_panel; // ссылка на панель встречной записки
    public GameObject Advice_for_dog; // ссылка на панель с советами для каждой собаки
    private string dog_id; // переменная с ID собаки
    private List<int> nodes_for_small_dog = new List<int>(); // лист нод для маленькой задачи
    private List<int> nodes_for_average_dog = new List<int>(); // лист нод для средней собаки (корги, паг или двортерьер)
    private List<int> nodes_for_big_dog = new List<int>(); // лист нод для большой собаки

    [System.Serializable] // чтобы в инспекторе назначать
    public class dialogue
    {
        public string npc_text; // текст нпс
        public string answer_variant; // варианты ответа для игрока 
        public int next_node; // назначаем следующую ноду
    }

    public dialogue[] nodes; // массив нод
    public int curr_node; // текущая нода для диалога
    public int curr_node_from_advice; // текущая нода для совета
    public TextMeshProUGUI npc_text; // ссылка на текст продавца
    public TextMeshProUGUI advice_text;
    public Button start_dialogue; // кнопка начала, "получить совет"
    public Button exit_dialogue; // кнопка выхода на панели начала игры
    public Button Answer_1; // кнопка - ответ для диалога
    public Button Answer_2; // кнпка выход из диалога
    public Button advice; // кнопка принятия совета
    public Button exit_advice; // кнопка выхода из панели советов

    private enum Action { Eat, Walk, Happy, Idle } // состояния 
    void Start()
    {
        dog_id = PlayerPrefs.GetString("SelectedDogID", ""); // получаем породу собаки
        List<int> curr_nodes_for_dog = choose_node_for_dog(); // список нод для советов для определённой собаки
        curr_node_from_advice = curr_nodes_for_dog[0]; // первая нода - её буду назначать
        //Debug.Log(dog_id);
        Debug.Log(curr_node_from_advice);
        hunger = PlayerPrefs.GetFloat("Hunger", 0f); // вывод текущего гоолода
        energy = PlayerPrefs.GetFloat("Walk", 0f); // вывод текущего желания погулять
        mood = PlayerPrefs.GetFloat("Happiness", 0f); // вывод текущего счастья

        //foreach (int num in curr_nodes_for_dog)
        //{
        //    Debug.Log(num);
        //}
        //Debug.Log($"Голод: {hunger}");
        //Debug.Log($"Выгул: {energy}");
        //Debug.Log($"Счастье: {mood}");
        Hello_panel.SetActive(true); // открываем панель приветствия
        Dialogue.SetActive(false); // скрываем для начала диалоговую панель
        Advice_for_dog.SetActive(false); // скрываем панель для совета
        //Button start_button = Hello_panel.transform.Find("start_dialogue").GetComponent<Button>();
        //Button exit_button = Hello_panel.transform.Find("exit_dialogue").GetComponent<Button>();
        start_dialogue.onClick.AddListener(start_dialogue_function); // кнопка старта диалога выполняет функцию начала диалога
        exit_dialogue.onClick.AddListener(exit_hello_function); // кнопка выхода из приветственной панели выполняет функцию закрытия панели

        Answer_1.onClick.AddListener(start_advice_function); // выбор диалога дальше после совета
        Answer_2.onClick.AddListener(exit_dialogue_function); // кнопка выхода из диалоговой панели выполняет функцию закрытия панели

        advice.onClick.AddListener(select_answer_from_advice); // кнопка отвечает за выполнение функции (смена нод далее по диалогу)
        exit_advice.onClick.AddListener(exit_advice_function); // кнопка выхода из панели советов выполняет функцию закрытия панели

        //show_dialogue();
    }

    void start_dialogue_function()
    {
        Hello_panel.SetActive(false); // выключается приветственная панель
        Dialogue.SetActive(true); // включается диалоговая панель
        //Advice_for_dog.SetActive(false); // выключается 
        int utility_ai_node = choose_best_action(); // берём ноду которая назначается с помощью utility ai в зависимости от того
                                                    // что хочет собака
        curr_node = utility_ai_node; // она становистя текущей
        //show_dialogue(); 
        npc_text.text = nodes[curr_node].npc_text; // текст слов нпс - его текущая реплика 
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
        advice_text.text = nodes[curr_node].npc_text; // текст слов нпс - его текущая реплика 
        advice.GetComponentInChildren<TextMeshProUGUI>().text = nodes[curr_node].answer_variant;
    }

    void exit_dialogue_function()
    {
        Dialogue.SetActive(false); // закрытие диалогового окна
    }

    void exit_advice_function()
    {
        Advice_for_dog.SetActive(false); // закрытие окна советов
    }

    void exit_hello_function()
    {
        Hello_panel.SetActive(false); // закрытие приветственного окна
    }

    void select_answer()
    {
        if (nodes[curr_node].next_node == -1 || nodes[curr_node].next_node == 0) // если нода = -1 или 0, то панель закрывается 
        {
            exit_dialogue_function();
            return;
        }
        curr_node = nodes[curr_node].next_node; // следующая нода
        show_dialogue();
    }

    void select_answer_from_advice() // для советов
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
        npc_text.text = nodes[curr_node].npc_text; // текст слов нпс - его текущая реплика 
        advice.GetComponentInChildren<TextMeshProUGUI>().text = nodes[curr_node].answer_variant; // назначение текста кнопке, меняется со сменой ноды
    }

    List<int> choose_node_for_dog()
    {
        dog_id = PlayerPrefs.GetString("SelectedDogID", ""); // узнаём, какая у нас собака
        if (dog_id == "chihuahua") // пополняем лист нод для чихуа
        {
            nodes_for_small_dog.Add(5);
            nodes_for_small_dog.Add(6);
            nodes_for_small_dog.Add(7);
            return nodes_for_small_dog;
            //return next_node;
        }
        else if (dog_id == "cur" || dog_id == "corgi" || dog_id == "pug") // пополняем лист нод для средних собак
        {
            nodes_for_average_dog.Add(8);
            nodes_for_average_dog.Add(6);
            nodes_for_average_dog.Add(10);
            return nodes_for_average_dog;
        }
        else // пополняем лист нод для большой собаки
        {
            nodes_for_big_dog.Add(9);
            nodes_for_big_dog.Add(6);
            nodes_for_big_dog.Add(11);
            return nodes_for_big_dog;
        }
    }

    int choose_best_action()
    {
        float hunger_score = hunger / 100f; // важность поесть
        float energy_score = energy / 100f; // важность погулять (меньше,чем поесть)
        float mood_score = mood / 100f; // важность порадоваться
        //float priority_1;
        //float priority_2;
        //float priority_3; // можно сделать выбор другой покупки, если не понравился первый совет (опционально)
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