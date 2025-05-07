using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class WalkManager : MonoBehaviour
{
    public Slider walkSlider;
    public Button setMaxWalkButton;
    public Button setMinWalkButton;

    private float walkValue;
    private const float maxWalk = 100f;
    private const float walkGainPerSecond = 1.2f;
    private const float walkLossPerHour = 10f;
    private float walkLossPerSecond;

    private float timeSinceLastUpdate = 0f;

    void Start()
    {
        walkLossPerSecond = walkLossPerHour / 3600f;

        LoadWalk();
        UpdateUI();

        setMaxWalkButton.onClick.AddListener(SetMaxWalk);
        setMinWalkButton.onClick.AddListener(SetMinWalk);
    }

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= 1f)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == "Park" || sceneName == "Playground")
            {
                walkValue += walkGainPerSecond * timeSinceLastUpdate;
            }
            else
            {
                walkValue -= walkLossPerSecond * timeSinceLastUpdate;
            }

            walkValue = Mathf.Clamp(walkValue, 0f, maxWalk);
            timeSinceLastUpdate = 0f;

            SaveWalk();
            UpdateUI();
        }
    }

    void LoadWalk()
    {
        walkValue = PlayerPrefs.GetFloat("Walk", 0f);
        string lastTimeString = PlayerPrefs.GetString("LastWalkTime", "");

        if (!string.IsNullOrEmpty(lastTimeString))
        {
            DateTime lastCheck = DateTime.Parse(lastTimeString);
            TimeSpan timePassed = DateTime.Now - lastCheck;

            float hoursPassed = (float)timePassed.TotalHours;
            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName != "Park" && sceneName != "Playground")
            {
                walkValue -= hoursPassed * walkLossPerHour;
                walkValue = Mathf.Clamp(walkValue, 0f, maxWalk);
            }
        }
    }

    void SaveWalk()
    {
        PlayerPrefs.SetFloat("Walk", walkValue);
        PlayerPrefs.SetString("LastWalkTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    void UpdateUI()
    {
        walkSlider.value = walkValue;
    }

    void SetMaxWalk()
    {
        walkValue = maxWalk;
        SaveWalk();
        UpdateUI();
        Debug.Log("Óñòàíîâëåí ìàêñèìóì ïðîãóëêè (100).");
    }

    void SetMinWalk()
    {
        walkValue = 0f;
        SaveWalk();
        UpdateUI();
        Debug.Log("Óñòàíîâëåí ìèíèìóì ïðîãóëêè (0).");
    }

    void OnApplicationQuit()
    {
        SaveWalk();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveWalk();
    }
}
