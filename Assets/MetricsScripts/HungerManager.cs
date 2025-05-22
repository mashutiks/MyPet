
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HungerManager : MonoBehaviour
{
    public Slider hungerSlider;
    public Button feedButton;


    private float hunger;
    private const float maxHunger = 100f;
    private const float feedAmount = 100f;
    private const float hungerLossPerHour = 6.25f;
    private float hungerLossPerSecond;
    private bool isFeeding = false; 

    private float timeSinceLastDecrease = 0f;

    void Start()
    {

        feedButton.onClick.AddListener(FeedPet);

    }

    void Awake()
    {
        hungerLossPerSecond = hungerLossPerHour / 3600f;
        LoadHunger();
        StartCoroutine(DelayedUIUpdate());
    }

    IEnumerator DelayedUIUpdate()
    {
        yield return null; 
        UpdateUI();        
    }



    void Update()
    {
        if (isFeeding) return;

        timeSinceLastDecrease += Time.deltaTime;

        if (timeSinceLastDecrease >= 1f)
        {
            hunger -= hungerLossPerSecond * timeSinceLastDecrease;
            hunger = Mathf.Clamp(hunger, 0f, maxHunger);
            timeSinceLastDecrease = 0f;

            UpdateUI();
        }
    }

    void LoadHunger()
    {
        hunger = PlayerPrefs.GetFloat("Hunger", maxHunger);
        string lastTimeString = PlayerPrefs.GetString("LastCheckTime", "");

        if (!string.IsNullOrEmpty(lastTimeString))
        {
            DateTime lastCheck = DateTime.Parse(lastTimeString);
            TimeSpan timePassed = DateTime.Now - lastCheck;

            float hoursPassed = (float)timePassed.TotalHours;
            hunger -= hoursPassed * hungerLossPerHour;
            hunger = Mathf.Clamp(hunger, 0f, maxHunger);
        }
    }

    void SaveHunger()
    {
        PlayerPrefs.SetFloat("Hunger", hunger);
        PlayerPrefs.SetString("LastCheckTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    void UpdateUI()
    {
        bool allFlagsZero = PlayerPrefs.GetInt("Item_Food1_Selected", 0) == 0 &&
                            PlayerPrefs.GetInt("Item_Food2_Selected", 0) == 0 &&
                            PlayerPrefs.GetInt("Item_Food3_Selected", 0) == 0;

        hungerSlider.value = hunger;
        feedButton.interactable = hunger <= 50f && !isFeeding && !allFlagsZero;
    }


    void FeedPet()
    {
        if (hunger <= 50f && !isFeeding)
        {
            StartCoroutine(FeedAnimation());
            Debug.Log("Ïèòîìåö íà÷àë åñòü!");
        }
    }

    IEnumerator FeedAnimation()
    {
        isFeeding = true;
        feedButton.interactable = false;

        float startHunger = hunger;
        float targetHunger = Mathf.Clamp(hunger + feedAmount, 0f, maxHunger);
        float duration = 13f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            hunger = Mathf.Lerp(startHunger, targetHunger, elapsed / duration);
            UpdateUI();
            elapsed += Time.deltaTime;
            yield return null;
        }

        hunger = targetHunger;
        UpdateUI();
        SaveHunger();
        isFeeding = false;
        Debug.Log("Ïèòîìåö çàêîí÷èë åñòü!");

        AchievemenetManager.Instance.EarnAchievement("Все, кроме голодовки");
    }


    void OnApplicationQuit()
    {
        SaveHunger();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveHunger();
    }
}