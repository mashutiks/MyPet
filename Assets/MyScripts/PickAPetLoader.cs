using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickAPetLoader : MonoBehaviour
{
    void Awake()
    {
        if (PlayerPrefs.GetInt("DogSelected", 0) == 0)
        {
            SceneManager.LoadScene("Pick_a_pet");
        }
    }
}