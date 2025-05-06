using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievemenetManager : MonoBehaviour
{
    public GameObject achievementPrefab;

    public Sprite[] sprites;

    private AchievementCategoryButtons activeButton;

    public ScrollRect ScrollRect;

    public GameObject achievementMenu;

    // Start is called before the first frame update
    void Start()
    {
        activeButton = GameObject.Find("generalButton").GetComponent<AchievementCategoryButtons>();
        CreateAchievement("general", "Test Title", "Hello", 15, 0);
        CreateAchievement("general", "Test Title", "Hello", 15, 0);
        CreateAchievement("general", "Test Title", "Hello", 15, 0);
        CreateAchievement("general", "Test Title", "Hello", 15, 0);
        CreateAchievement("general", "Test Title", "Hello", 15, 0);

        CreateAchievement("others", "Test Title", "Hello", 15, 0);
        CreateAchievement("others", "Test Title", "Hello", 15, 0);
        CreateAchievement("others", "Test Title", "Hello", 15, 0);
        CreateAchievement("others", "Test Title", "Hello", 15, 0);
        CreateAchievement("others", "Test Title", "Hello", 15, 0);

        foreach (GameObject achievementList in GameObject.FindGameObjectsWithTag("AchievementList"))
        {
            achievementList.SetActive(false);
        }
        activeButton.Click();

        achievementMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateAchievement(string category, string title, string description, int points, int spriteIndex)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);
        SetAchievementInfo(category, achievement, title, description, points, spriteIndex);
    }

    public void SetAchievementInfo(string category, GameObject achievement, string title, string description, int points, int spriteIndex)
    {
        achievement.transform.SetParent(GameObject.Find(category).transform, false);

        Transform titleTransform = achievement.transform.Find("title");
        Transform descriptionTransform = achievement.transform.Find("description");
        Transform coinsTransform = achievement.transform.Find("points");
        Transform pictureTransform = achievement.transform.Find("picture");

        if (titleTransform != null)
            titleTransform.GetComponent<TextMeshProUGUI>().text = title;

        if (descriptionTransform != null)
            descriptionTransform.GetComponent<TextMeshProUGUI>().text = description;

        if (coinsTransform != null)
            coinsTransform.GetComponent<TextMeshProUGUI>().text = points.ToString();

        if (pictureTransform != null)
            pictureTransform.GetComponent<Image>().sprite = sprites[spriteIndex];

    }

    public void ChangeCategory(GameObject button)
    {
        AchievementCategoryButtons achievementCategoryButtons = button.GetComponent<AchievementCategoryButtons>();
        ScrollRect.content = achievementCategoryButtons.achievementList.GetComponent<RectTransform>();

        achievementCategoryButtons.Click();
        activeButton.Click();
        activeButton = achievementCategoryButtons;
    }
}
