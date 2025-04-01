using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;

        // Pause all audio
        AudioListener.pause = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        // Resume all audio
        AudioListener.pause = false;
    }
}
