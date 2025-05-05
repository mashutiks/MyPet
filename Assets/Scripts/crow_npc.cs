using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
public class crow_npc : MonoBehaviour
{
    private Transform dog; // позиция пса (пока только гг)
    private Animator animator;
    [Range(0, 100)] private float hunger = 50f; // голод
    [Range(0, 100)] private float danger = 0f; // опасность (растёт по мере приближения пса)
    [Range(0, 100)] private float dirtyness = 50f; // чистота 
    private float danger_decrease = 90f; // насколько уменьшается опасность после того как птица улетит
    private float hunger_decrease = 30f; // насколько уменьшаяется голод, после того как поест
    private float dirtyness_decrease = 30f; // насколько уменьшается грязь с тела когда птица чистится 
    private float hunger_increase = 5f; // в спокойном состоянии голод растёт на 5
    private float dirtyness_increase = 5f; // в спокойном состоянии грязь растёт на 5
    public float speed = 4f; // скорость полёта
    public float height = 3f; // максимальная высота полёта
    public float distance = 5f; // радиус на который птица может отлететь
    public float duration_fly = 2f; // время полёта
    public float max_distance_to_dog = 3f; // максимальное расстояние до собаки
    public float danger_distance_to_dog = 2f; // опасное расстояние до собаки 
    private Vector3 start_pos; // начальное положение птицы
    private Vector3 finish_pos; // конечное положение птицы
    private float fly_time; // счётчик времени 
    private bool is_moving = false; // флажок
    private enum Action { Eat, Clean, Idle, Fly } // состояния 
    void Start()
    {
        dog = GameObject.FindGameObjectWithTag("Player").transform; // игровой объект пса
        animator = GetComponent<Animator>();
        is_moving = false;
    }

    void Update()
    {
        animator.SetBool("flying", false); // изначально птица не летит
        if (!is_moving)
        {
            Action best_action = choose_best_action(); // выбор лучшего действия 
            Debug.Log($"{best_action} | Опасность: {danger} | Голод: {hunger} | Грязь: {dirtyness}"); // логи
            StartCoroutine(calculations(best_action)); // корутин нужен для того, чтобы анимации успевали проигрываться  
        }
    }

    private Action choose_best_action()
    {
        Vector3 crow_pos = transform.position; // позиция птицы
        Vector3 dog_pos = dog.position; // позиция пса
        float distance_to_dog = (crow_pos - dog_pos).magnitude; // расстояние между псом и птицей 
        float eat_score = hunger / 100f * 2f; // важность поесть
        float clean_score = dirtyness / 100f; // важность почиститься (меньше,чем поесть)
        float idle_score = 0.1f; // изначально маленькое  
        if (distance_to_dog <= 2f) 
        {
            danger = 100f; // если собака ближе чем на 2 метра, то приоритет взлёта поднимается до 100
        }
        float danger_score = danger / 100f; // аналогично с опасностью
        if (danger_score >= eat_score && danger_score > clean_score && danger_score > idle_score)
        {
            return Action.Fly;
        }
        else if (eat_score >= clean_score && eat_score > danger_score && eat_score > idle_score)
        {
            return Action.Eat;
        }
        else if (clean_score > eat_score && clean_score > danger_score && clean_score > idle_score)
        {
            return Action.Clean;
        }
        else
        {
            return Action.Idle;
        }
    }

    private IEnumerator calculations(Action action)
    {
        is_moving = true;
        switch (action)
        {
            case Action.Fly:
                //animator.SetBool("flying", true);
                fly_time = 0f;
                // тут птица поворачивается в рандомную сторону чтобы улететь туда
                Vector3 random_direction = Random.insideUnitSphere * 3f; // выбираетяс рандомная точка в сфере радиуса 3
                random_direction += transform.position;
                UnityEngine.AI.NavMeshHit point;
                UnityEngine.AI.NavMesh.SamplePosition(random_direction, out point, 3f, UnityEngine.AI.NavMesh.AllAreas); 
                Vector3 direction = (point.position - transform.position).normalized; // расстояние до той точки
                Quaternion look_rotation = Quaternion.LookRotation(direction); // поворот в эту сторону 
                transform.rotation = Quaternion.Slerp(transform.rotation, look_rotation, Time.deltaTime * 10f);
                //transform.position = point.position;
                
                danger = Mathf.Max(0, danger - danger_decrease);
                break;
            case Action.Eat:
                animator.SetTrigger("peck");
                yield return new WaitForSeconds(1.5f);
                hunger = Mathf.Max(0, hunger - hunger_decrease);
                break;
            case Action.Clean:
                int random_animation = Random.Range(0, 2);
                if (random_animation == 0)
                {
                    animator.SetTrigger("preen");
                    yield return new WaitForSeconds(1.5f);
                }
                else if (random_animation == 1)
                {
                    animator.SetTrigger("ruffle");
                    yield return new WaitForSeconds(1.5f);
                }
                dirtyness = Mathf.Max(0, dirtyness - dirtyness_decrease);
                break;
            case Action.Idle:
                yield return new WaitForSeconds(1.5f);
                hunger = Mathf.Min(100, hunger + hunger_increase);
                dirtyness = Mathf.Min(100, dirtyness + dirtyness_increase);
                break;
        }
        is_moving = false;
    }
   
}
