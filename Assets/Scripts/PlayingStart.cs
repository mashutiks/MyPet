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
    private Vector3 stick_position; // куда поставим палку при начале игры
    public Button PlayingButton; // кнопка "Играть"
    public Vector3 initial_scale; // начальный размер палки
    private string dog_id; // переменная с ID собаки
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
    }

    void SetStickPosition()
    {
        dog_id = PlayerPrefs.GetString("SelectedDogID", ""); // узнаём, какая у нас собака
        stick.transform.SetParent(null); // возвращаем палку обратно
        stick.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        if (dog_id == "pug" || dog_id == "germanshepherd") // в зависимости от вида собаки чуть увеличиваем длину палки
        {
            stick.transform.localScale = initial_scale;
            stick.transform.localScale = new Vector3(initial_scale.x, initial_scale.y * 1.5f, initial_scale.z);
        }
        stick_position = new Vector3(-6.69999981f, 2.55999994f, 40f);
        stick.transform.position = stick_position; // устанавливаем палку перед камерой
        PlayingButton.interactable = false; // блокируем кнопку
    }
}