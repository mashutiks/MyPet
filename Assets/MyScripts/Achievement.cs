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
            achievementRef.GetComponent<Image>().sprite = AchievemenetManager.Instanse.unlockedSprite;
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

            Unlocked = true;
            return true;
        }
        return false;
    }

}
