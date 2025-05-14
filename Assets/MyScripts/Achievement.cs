using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Achievement
{
    private string name;

    private string description;

    private bool unlocked;

    private int points;

    private int spriteIndex;

    private int bronzeProgression;
    private int silverProgression;
    private int goldProgression;

    private int currentProgression;

    private int maxProgression;

    private GameObject achievementRef;

    public Achievement(string name, string description, int points, int spriteIndex, GameObject achievementRef, int bronzeProgression, int silverProgression, int goldProgression)
    {
        this.Name = name;
        this.Description = description;
        this.Unlocked = false;
        this.Points = points;
        this.SpriteIndex = spriteIndex;
        this.achievementRef = achievementRef;
        this.maxProgression = bronzeProgression;
        this.bronzeProgression = bronzeProgression;
        this.silverProgression = silverProgression;
        this.goldProgression = goldProgression;

        if (PlayerPrefs.HasKey("MaxProgression" + Name)) this.maxProgression = PlayerPrefs.GetInt("MaxProgression" + Name);
        else this.maxProgression = bronzeProgression;


        if (PlayerPrefs.HasKey("Progression" + Name)) currentProgression = PlayerPrefs.GetInt("Progression" + Name);
        else this.currentProgression = 0;
        LoadAchievement();
    }

    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public bool Unlocked { get => unlocked; set => unlocked = value; }
    public int Points { get => points; set => points = value; }
    public int SpriteIndex { get => spriteIndex; set => spriteIndex = value; }


    public int CurrentProgression { get => currentProgression; set => currentProgression = value; }
    public int MaxProgression { get => maxProgression; set => maxProgression = value; }

    public int BronzeProgression { get => bronzeProgression; set => bronzeProgression = value; }

    public int SilverProgression { get => silverProgression; set => silverProgression = value; }

    public int GoldProgression { get => goldProgression; set => goldProgression = value; }

    public bool EarnAchievement()
    {
        if (!Unlocked && CheckProgress())
        {

            if (maxProgression == goldProgression)
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
            }


            SaveAchievement(true);
            return true;
        }
        return false;
    }

    public void SaveAchievement(bool value)
    {
        Unlocked = value;

        if (value)
        {
            int tmpPoints = PlayerPrefs.GetInt("Points");
            PlayerPrefs.SetInt("Points", tmpPoints += points);
        }

        PlayerPrefs.SetInt("Progression" + Name, currentProgression);

        PlayerPrefs.SetInt(name, value ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void LoadAchievement()
    {
        unlocked = PlayerPrefs.GetInt(name) == 1 ? true : false;


        if (unlocked)
        {
            AchievemenetManager.Instance.textPoints.text = "";

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

    public bool CheckProgress()
    {
        if (maxProgression > 0)
        {
            currentProgression++;

            achievementRef.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Name + " " + currentProgression + "/" + maxProgression;
        }


        SaveAchievement(false);


        if (maxProgression == 0)
        {
            return true;
        }
        if (currentProgression >= maxProgression)
        {
            return true;
        }
        return false;
    }

    public void TransformMaxProgression()
    {
        achievementRef.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Name + " " + currentProgression + "/" + maxProgression;

        achievementRef.transform.GetChild(6).GetComponent<Image>().color = new Color(1, 1, 1, 1);

        if (currentProgression == bronzeProgression) achievementRef.transform.GetChild(6).GetComponent<Image>().sprite = AchievemenetManager.Instance.bronzeMedal;
        if (currentProgression == silverProgression) achievementRef.transform.GetChild(6).GetComponent<Image>().sprite = AchievemenetManager.Instance.silverMedal;
        if (currentProgression == goldProgression) achievementRef.transform.GetChild(6).GetComponent<Image>().sprite = AchievemenetManager.Instance.goldMedal;
    }
}