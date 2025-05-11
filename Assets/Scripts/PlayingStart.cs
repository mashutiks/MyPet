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
        stick.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        stick_position = new Vector3(-6.69999981f, 2.55999994f, 40f);
        stick.transform.position = stick_position; // устанавливаем палку перед камерой
    }
}