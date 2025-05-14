using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayingStart : MonoBehaviour
{
    public GameObject stick; // палка-игрушка
    public GameObject bone; // косточка-игрушка
    public GameObject fish; // рыбка-игрушка
    private Vector3 active_toy_position; // куда поставим палку при начале игры
    public Button PlayingButton; // кнопка "Играть"
    public Vector3 initial_scale; // начальный размер палки
    private string dog_id; // переменная с ID собаки

    private GameObject active_toy; // текущая активная игрушка
    void Start()
    {
        if (PlayingButton != null)
        {
            PlayingButton.onClick.AddListener(SetStickPosition);
        }
        else
        {
            Debug.LogWarning("Кнопка 'Играть' не назначена!");
        }

        CheckItemAvailability();
    }


    void SetStickPosition()
    {
        if (active_toy == null)
        {
            return;
        }

        dog_id = PlayerPrefs.GetString("SelectedDogID", ""); // узнаём, какая у нас собака
        active_toy.transform.SetParent(null); // возвращаем палку обратно
        active_toy.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        if (dog_id == "pug" || dog_id == "germanshepherd") // в зависимости от вида собаки чуть увеличиваем длину палки
        {
            active_toy.transform.localScale = initial_scale;
            active_toy.transform.localScale = new Vector3(initial_scale.x, initial_scale.y * 1.5f, initial_scale.z);
        }
        active_toy_position = new Vector3(-6.69999981f, 2.55999994f, 40f);
        active_toy.transform.position = active_toy_position; // устанавливаем палку перед камерой
        PlayingButton.interactable = false; // блокируем кнопку
    }
    void CheckItemAvailability()
    {
        if (stick != null)
        {
            stick.SetActive(false);
        }
        if (bone != null)
        {
            bone.SetActive(false);
        }
        if (fish != null)
        {
            fish.SetActive(false);
        }

        int stick_ = PlayerPrefs.GetInt("Item_Stick_Selected", 0);
        int bone_ = PlayerPrefs.GetInt("Item_Bone_Selected", 0);
        int fish_ = PlayerPrefs.GetInt("Item_Fish_Selected", 0);

        bool hasItem = (stick_ == 1 || bone_ == 1 || fish_ == 1);

        if (PlayingButton != null)
        {
            PlayingButton.interactable = hasItem;
        }

        if (stick_ == 1 && stick != null)
        {
            stick.SetActive(true);
            active_toy = stick;
        }
        else if (bone_ == 1)
        {
            bone.SetActive(true);
            active_toy = bone;
        }
        else if (fish_ == 1)
        {
            fish.SetActive(true);
            active_toy = fish;
        }
    }
}



