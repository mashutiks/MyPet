using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class intro_window : MonoBehaviour
{
    public GameObject Intro;

    public void close_window()
    {
        if (Intro != null)
        {
            Intro.SetActive (false);
        }
    }
}
