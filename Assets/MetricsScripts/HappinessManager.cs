using UnityEngine;
using UnityEngine.UI;
using System;

public class HappinessManager : MonoBehaviour
{
    public Slider happinessSlider;
    public Button setMaxHappinessButton;
    public Button setMinHappinessButton;

    private float happiness;
    private const float maxHappiness = 100f;
    private const float happinessChangePerHour = 1f;
    //private const float happinessChangePerHour = 100f;
    private float happinessChangePerSecond;

    private float timeSinceLastUpdate = 0f;

    void Start()
    {
        happinessChangePerSecond = happinessChangePerHour / 3600f;

        LoadHappiness();
        UpdateUI();

        setMaxHappinessButton.onClick.AddListener(SetMaxHappiness);
        setMinHappinessButton.onClick.AddListener(SetMinHappiness);
    }

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= 1f)
        {
            float hunger = PlayerPrefs.GetFloat("Hunger", 0f);
            float walk = PlayerPrefs.GetFloat("Walk", 0f);

            if (hunger > 70f && walk > 70f)
            {
                happiness += happinessChangePerSecond * timeSinceLastUpdate;
            }
            else if (hunger < 20f || walk < 20f)
            {
                happiness -= happinessChangePerSecond * timeSinceLastUpdate;
            }

            happiness = Mathf.Clamp(happiness, 0f, maxHappiness);
            timeSinceLastUpdate = 0f;

            SaveHappiness();
            UpdateUI();
        }
    }

    void LoadHappiness()
    {
        happiness = PlayerPrefs.GetFloat("Happiness", maxHappiness);
        string lastTimeString = PlayerPrefs.GetString("LastHappinessCheckTime", "");

        if (!string.IsNullOrEmpty(lastTimeString))
        {
            DateTime lastCheck = DateTime.Parse(lastTimeString);
            TimeSpan timePassed = DateTime.Now - lastCheck;
            float secondsPassed = (float)timePassed.TotalSeconds;

            float hunger = PlayerPrefs.GetFloat("Hunger", 0f);
            float walk = PlayerPrefs.GetFloat("Walk", 0f);

            if (hunger > 70f && walk > 70f)
                happiness += happinessChangePerSecond * secondsPassed;
            else if (hunger < 30f || walk < 30f)
                happiness -= happinessChangePerSecond * secondsPassed;

            happiness = Mathf.Clamp(happiness, 0f, maxHappiness);
        }
    }

    void SaveHappiness()
    {
        PlayerPrefs.SetFloat("Happiness", happiness);
        PlayerPrefs.SetString("LastHappinessCheckTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    void UpdateUI()
    {
        happinessSlider.value = happiness;
    }

    void SetMaxHappiness()
    {
        happiness = maxHappiness;
        SaveHappiness();
        UpdateUI();
        Debug.Log("—частье установлено на максимум (100).");
    }

    void SetMinHappiness()
    {
        happiness = 0f;
        SaveHappiness();
        UpdateUI();
        Debug.Log("—частье установлено на минимум (0).");
    }

    void OnApplicationQuit()
    {
        SaveHappiness();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveHappiness();
    }
}
