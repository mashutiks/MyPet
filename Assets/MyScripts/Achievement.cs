using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement
{
    private string name;

    private string description;

    private bool unlocked;

    private int points;

    private int spriteIndex;

    private GameObject achievementRef;

    public Achievement(string name, string description, int points, int spriteIndex, GameObject achievementRef)
    {
        this.Name = name;
        this.Description = description;
        this.Unlocked = false;
        this.Points = points;
        this.SpriteIndex = spriteIndex;
        this.achievementRef = achievementRef;

        LoadAchievement();
    }

    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public bool Unlocked { get => unlocked; set => unlocked = value; }
    public int Points { get => points; set => points = value; }
    public int SpriteIndex { get => spriteIndex; set => spriteIndex = value; }

    public bool EarnAchievement()
    {
        if (!Unlocked)
        {
            achievementRef.GetComponent<Image>().sprite = AchievemenetManager.Instance.unlockedSprite;
            achievementRef.GetComponent<Image>().color = Color.black;

            // Убираем изображение с названием "coin"
            Transform coinTransform = achievementRef.transform.Find("coins");
            if (coinTransform != null)
            {
                coinTransform.gameObject.SetActive(false);
            }

            // Убираем текст с названием "points"
            Transform pointsTransform = achievementRef.transform.Find("points");
            if (pointsTransform != null)
            {
                pointsTransform.gameObject.SetActive(false);
            }

            SaveAchievement(true);
            return true;
        }
        return false;
    }

    public void SaveAchievement(bool value)
    {
        Unlocked = true;

        int tmpPoints = PlayerPrefs.GetInt("Points");
        PlayerPrefs.SetInt("Points", tmpPoints += points);

        PlayerPrefs.SetInt(name, value ? 1 : 0);

        Debug.Log($"{name}: {value}");

        PlayerPrefs.Save();
    }

    public void LoadAchievement()
    {
        unlocked = PlayerPrefs.GetInt(name) == 1 ? true : false;

        if (unlocked)
        {
            AchievemenetManager.Instance.textPoints.text = "Points: " + PlayerPrefs.GetInt("Points");

            achievementRef.GetComponent<Image>().sprite = AchievemenetManager.Instance.unlockedSprite;
            achievementRef.GetComponent<Image>().color = Color.black;

            // Убираем изображение с названием "coin"
            Transform coinTransform = achievementRef.transform.Find("coins");
            if (coinTransform != null)
            {
                coinTransform.gameObject.SetActive(false);
            }

            // Убираем текст с названием "points"
            Transform pointsTransform = achievementRef.transform.Find("points");
            if (pointsTransform != null)
            {
                pointsTransform.gameObject.SetActive(false);
            }
        }
    }


}