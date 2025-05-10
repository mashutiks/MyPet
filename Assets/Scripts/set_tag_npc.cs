using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_tag_npc : MonoBehaviour
{
    private Transform player; // позиция гг
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        Vector3 npc_pos = transform.position;
        Vector3 dog_pos = player.position;
        float distance_dog_npc = (npc_pos - dog_pos).magnitude;
        if (distance_dog_npc <= 2f && !gameObject.CompareTag("enemy"))
        {
            gameObject.tag = "enemy";
        }
        else if (distance_dog_npc > 2f && gameObject.CompareTag("enemy"))
        {
            gameObject.tag = "dog";
        }
    }
}
