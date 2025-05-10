using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class crow_npc : MonoBehaviour
{
    private Transform dog; // позиция пса (пока только гг)
    //private GameObject dogs;
    //private Transform enemy; // позиция нпс псов
    private Animator animator;
    [Range(0, 100)] private float hunger = 50f; // голод
    [Range(0, 100)] private float danger = 0f; // опасность (растёт по мере приближения пса)
    [Range(0, 100)] private float dirtyness = 50f; // чистота 
    //private float danger_decrease = 90f; // насколько уменьшается опасность после того как птица улетит
    private float hunger_decrease = 30f; // насколько уменьшаяется голод, после того как поест
    private float dirtyness_decrease = 30f; // насколько уменьшается грязь с тела когда птица чистится 
    private float hunger_increase = 5f; // в спокойном состоянии голод растёт на 5
    private float dirtyness_increase = 5f; // в спокойном состоянии грязь растёт на 5
    public float height = 1f; // максимальная высота полёта
    public float danger_distance_to_dog = 2f; // опасное расстояние до собаки 
    private bool is_moving = false; // флажок

    private enum Action { Eat, Clean, Idle, Fly } // состояния 
    void Start()
    {
        dog = GameObject.FindGameObjectWithTag("Player").transform; // игровой объект пса
        //startPos = transform.position;

        animator = GetComponent<Animator>();
        animator.SetBool("flying", false); // изначально птица не летит
        animator.SetBool("landing", false); // изначально птица не летит
        is_moving = false;
        //dogs = GameObject.FindGameObjectWithTag("dog");
        //npc = GameObject.FindGameObjectWithTag("crow_enemy");
    }

    void Update()
    {
        //dogs = GameObject.FindGameObjectWithTag("dog");
        
        if (!is_moving)
        {
            Action best_action = choose_best_action(); // выбор лучшего действия 
            Debug.Log($"{best_action} | Опасность: {danger} | Голод: {hunger} | Грязь: {dirtyness}"); // логи
            StartCoroutine(calculations(best_action)); // корутин нужен для того, чтобы анимации успевали проигрываться
        }
    }

    private Action choose_best_action()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("dog");
        foreach (GameObject enemy in enemies)
        {
            float distance_to_dog = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance_to_dog <= danger_distance_to_dog)
            {
                danger = 100f;
                //return Action.Fly;
                //break;
            }
        }
        if (dog != null)
        {
            float distance_to_player = Vector3.Distance(transform.position, dog.transform.position);
            if (distance_to_player <= danger_distance_to_dog)
            {
                danger = 100f;
                //return Action.Fly;
                //break;
            }
        }
        //enemy = dogs.transform;
        //Vector3 crow_pos = transform.position; // позиция птицы
        //Vector3 dog_pos = enemy.position; // позиция пса
        //float distance_to_dog = (crow_pos - dog_pos).magnitude; // расстояние между псом и птицей
        //Debug.Log($"{distance_to_dog}");
        //if (distance_to_dog <= danger_distance_to_dog)
        //{
        //    gameObject.tag = "fly_away";
        //    danger = 100f; // если собака ближе чем на 2 метра, то приоритет взлёта поднимается до 100
        //}
        

        float eat_score = hunger / 100f * 2f; // важность поесть
        float clean_score = dirtyness / 100f; // важность почиститься (меньше,чем поесть)
        float idle_score = 0.1f; // изначально маленькое  

        float danger_score = danger / 100f; // аналогично с опасностью
        if (danger_score >= eat_score && danger_score > clean_score && danger_score > idle_score)
        {
            return Action.Fly;
        }
        else if (eat_score >= clean_score && eat_score > idle_score)
        {
            return Action.Eat;
        }
        else if (clean_score >= eat_score && clean_score > idle_score)
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
                //enemy = GameObject.FindGameObjectWithTag("enemy").transform;
                Vector3 random_direction = Random.insideUnitSphere * 4f; // выбираетяс рандомная точка в сфере радиуса 4
                random_direction += transform.position;
                UnityEngine.AI.NavMeshHit point;
                UnityEngine.AI.NavMesh.SamplePosition(random_direction, out point, 5f, UnityEngine.AI.NavMesh.AllAreas);
                Vector3 start_pos = transform.position;
                //Vector3 direction = (point.position - start_pos).normalized; // расстояние до той точки
                float random_circle = Random.Range(3f, 5f);
                if (Vector3.Distance(start_pos, point.position) < 3f)
                {
                    point.position = transform.position + (point.position - transform.position).normalized * random_circle;
                }

                Vector3 direction = (point.position - transform.position).normalized; // расстояние до той точки
                animator.SetBool("flying", true);
                yield return jumping(point);
                animator.SetBool("flying", false);
                animator.SetBool("landing", true);
                //Debug.Log($"{point.position}");
                danger = 0f;
                animator.SetBool("landing", false);
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
                int random_animation_in_idle = Random.Range(0, 2);
                if (random_animation_in_idle == 0 /*&& danger == 0*/)
                {
                    animator.SetTrigger("sing");
                    yield return new WaitForSeconds(1.5f);
                }
                else /*if (random_animation_in_idle == 1)*/
                {
                    animator.SetInteger("hop", 0);
                    yield return new WaitForSeconds(1.5f);
                }
                // todo: добавить прыжки в сторону
                //else if (random_animation_in_idle == 2)
                //{
                //    animator.SetInteger("hop", 1);
                //    yield return new WaitForSeconds(1.5f);
                //}
                //else if (random_animation_in_idle == 3)
                //{
                //    animator.SetInteger("hop", -1);
                //    yield return new WaitForSeconds(1.5f);
                //}
                //else
                //{
                //    animator.SetInteger("hop", -2);
                //    yield return new WaitForSeconds(1.5f);
                //}
                hunger = Mathf.Min(100, hunger + hunger_increase);
                dirtyness = Mathf.Min(100, dirtyness + dirtyness_increase);
                break;
        }
        is_moving = false;
    }

    private IEnumerator jumping(UnityEngine.AI.NavMeshHit finish_point)
    {
        transform.LookAt(finish_point.position);
        Vector3 start_pos = transform.position;
        float jumping_time = 0f;
        while (jumping_time < 2f)
        {
            float t = jumping_time / 2f;
            Vector3 x_z_direction = Vector3.Lerp(start_pos, finish_point.position, t);
            float y_direction = 1f * (-4 * t * t + 4 * t);
            transform.position = x_z_direction + Vector3.up * y_direction;
            //Vector3 direction = (finish_point - start_pos).normalized;
            jumping_time += Time.deltaTime;
            yield return null;
        }
        transform.position = finish_point.position;
        //animator.SetBool("flying", false);
    }

}