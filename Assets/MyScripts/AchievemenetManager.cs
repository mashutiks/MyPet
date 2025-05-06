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

    public GameObject visualAchievement;

    public GameObject achievementMenu;

    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

    // Start is called before the first frame update
    void Start()
    {
        activeButton = GameObject.Find("generalButton").GetComponent<AchievementCategoryButtons>();
        CreateAchievement("general", "Press W", "Press W to ulock this", 5, 0);

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
        if (Input.GetKeyDown(KeyCode.W))
        {
            EarnAchievement("Press W");
        }
    }

    public void EarnAchievement(string title)
    {
        if (achievements[title].EarnAchievement())
        {
            //DO SMTH AWESOME
            GameObject achievement = (GameObject)Instantiate(visualAchievement);
            SetAchievementInfo("EarnCanvas", achievement, title);
            StartCoroutine(HideAchievement(achievement));
        }
    }

    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);
    }

    public void CreateAchievement(string parent, string title, string description, int points, int spriteIndex)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);

        Achievement newAchievement = new Achievement(name, description, points, spriteIndex, achievement);
        
        achievements.Add(title, newAchievement);

        SetAchievementInfo(parent, achievement, title);
    }

    public void SetAchievementInfo(string parent, GameObject achievement, string title)
    {
        achievement.transform.SetParent(GameObject.Find(parent).transform, false);

        Transform titleTransform = achievement.transform.Find("title");
        Transform descriptionTransform = achievement.transform.Find("description");
        Transform coinsTransform = achievement.transform.Find("points");
        Transform pictureTransform = achievement.transform.Find("picture");

        if (titleTransform != null)
            titleTransform.GetComponent<TextMeshProUGUI>().text = title;

        if (descriptionTransform != null)
            descriptionTransform.GetComponent<TextMeshProUGUI>().text = achievements[title].Description;

        if (coinsTransform != null)
            coinsTransform.GetComponent<TextMeshProUGUI>().text = achievements[title].Points.ToString();

        if (pictureTransform != null)
            pictureTransform.GetComponent<Image>().sprite = sprites[achievements[title].SpriteIndex];

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
