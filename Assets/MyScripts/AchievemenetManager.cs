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

    public Sprite unlockedSprite;

    public TextMeshProUGUI textPoints;

    private static AchievemenetManager instance;

    private int fadeTime = 2;


    public static AchievemenetManager Instance 
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindAnyObjectByType<AchievemenetManager>();
            }
            return AchievemenetManager.instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //REMEMBERTOREMOVE
        PlayerPrefs.DeleteKey("Walking");
        PlayerPrefs.DeleteKey("Eating");
        PlayerPrefs.DeleteKey("Happy");
        PlayerPrefs.DeleteKey("Points");

        //PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); // Сохраняем изменения


        activeButton = GameObject.Find("generalButton").GetComponent<AchievementCategoryButtons>();
        //CreateAchievement("general", "Press W", "Press W to ulock this", 5, 0);

        CreateAchievement("general", "Eating", "Feed to ulock this", 10, 0);
        CreateAchievement("general", "Walking", "Walk to ulock this", 15, 1);
        CreateAchievement("general", "Happy", "Happy dog to ulock this", 15, 2);

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
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    EarnAchievement("Press W");
        //}
        if (PlayerPrefs.GetFloat("Walk") == 100f)
        {
            EarnAchievement("Walking");
        }
        if (PlayerPrefs.GetFloat("Hunger") == 100f)
        {
            EarnAchievement("Eating");
        }
        if (PlayerPrefs.GetFloat("Happiness") == 100f)
        {
            EarnAchievement("Happy");
        }

    }

    public void EarnAchievement(string title)
    {
        if (achievements[title].EarnAchievement())
        {
            //DO SMTH AWESOME
            GameObject achievement = (GameObject)Instantiate(visualAchievement);
            SetAchievementInfo("EarnCanvas", achievement, title);
            textPoints.text = "Points: " + PlayerPrefs.GetInt("Points");
            StartCoroutine(FadeAchievement(achievement));
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

        Achievement newAchievement = new Achievement(title, description, points, spriteIndex, achievement);
        
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

    private IEnumerator FadeAchievement(GameObject achievement)
    {
        CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup>();

        float rate = 1.0f / fadeTime;

        int startAlpha = 0;
        int endAlpha = 1;


        for (int i = 0; i<2; i++)
        {
            float progress = 0.0f;

            while (progress < 1.0f)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

                progress += rate * Time.deltaTime;

                yield return null;
            }

            yield return new WaitForSeconds(2);
            startAlpha = 1;
            endAlpha = 0;
        }

        Destroy(achievement);

    }
}
