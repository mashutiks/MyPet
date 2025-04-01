using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Awake()
    {
        if (PlayerPrefs.GetInt("DogSelected", 0) == 1)
        {
            SceneManager.LoadScene("Home");
        }
    }
}
