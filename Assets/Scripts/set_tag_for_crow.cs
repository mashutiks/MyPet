using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_tag_for_crow : MonoBehaviour
{
    private Transform crow; // позиция птицы
    void Start()
    {
        crow = GameObject.FindGameObjectWithTag("lb_bird").transform;
    }

    void Update()
    {
        Vector3 crow_pos = crow.position;
        Vector3 npc_pos = transform.position;
        float distance_to_dog_npc = (crow_pos - npc_pos).magnitude;
        if (distance_to_dog_npc <= 2f && !gameObject.CompareTag("crow_enemy"))
        {
            gameObject.tag = "crow_enemy";
        }
        else if (distance_to_dog_npc > 2f && gameObject.CompareTag("crow_enemy"))
        {
            gameObject.tag = "Untagged";
        }
    }
}
