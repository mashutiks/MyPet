using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievemenetManager : MonoBehaviour
{
    public GameObject achievementPrefab;

    public Sprite bronzeMedal; // Ссылка на бронзовую медаль
    public Sprite silverMedal; // Ссылка на серебряную медаль
    public Sprite goldMedal; // Ссылка на золотую медаль


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
        ////REMEMBERTOREMOVE
        //PlayerPrefs.DeleteKey("На свежем воздухе");
        //PlayerPrefs.DeleteKey("Всё, кроме голодовки");
        //PlayerPrefs.DeleteKey("Счастливое детство");

        //PlayerPrefs.DeleteKey("Прыжок в небеса");
        //PlayerPrefs.DeleteKey("Уклонение от пернатых");
        //PlayerPrefs.DeleteKey("Кошачьи ловушки");
        //PlayerPrefs.DeleteKey("На волоске от победы");
        //PlayerPrefs.DeleteKey("Победитель птиц и кошек");

        //PlayerPrefs.DeleteKey("ProgressionВсё, кроме голодовки");
        //PlayerPrefs.DeleteKey("MaxProgressionВсё, кроме голодовки");

        //PlayerPrefs.DeleteKey("Points");
        textPoints.text = "";

        //PlayerPrefs.DeleteAll(); //ПОТОМ ЗАКОММЕНТИТЬ!!!!!!
        PlayerPrefs.Save(); // Сохраняем изменения


        activeButton = GameObject.Find("generalButton").GetComponent<AchievementCategoryButtons>();

        CreateAchievement("general", "Все, кроме голодовки", "Накормить питомца", 10, 0, 1, 3, 5);
        CreateAchievement("general", "Лучше 100 друзей", "Познакомиться с другом", 10, 4, 1, 3, 5);

        CreateAchievement("general", "На свежем воздухе", "Прогулять питомца вдоволь", 15, 1, 0);
        CreateAchievement("general", "Счастливое детство", "Достичь максимального счастья питомца", 15, 2, 0);

        CreateAchievement("miniGames", "Прыжок в небеса", "Прошли первый уровень", 15, 3, 0);
        CreateAchievement("miniGames", "Уклонение от пернатых", "Допрыгали второй", 15, 3, 0);
        CreateAchievement("miniGames", "Кошачьи ловушки", "Преодолели три уровня", 15, 3, 0);
        CreateAchievement("miniGames", "На волоске от победы", "Справились с четыремя", 15, 3, 0);
        CreateAchievement("miniGames", "Победитель птиц и кошек", "Ура! Все пять уровней пройдены!", 15, 3, 0);

        CreateAchievement("others", "Заботливый хозяин", "Зашел в игру дней подряд", 10, 0, 3, 5, 10);



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
        if (PlayerPrefs.GetFloat("Walk") == 100f)
        {
            EarnAchievement("На свежем воздухе");
        }
        //if (PlayerPrefs.GetFloat("Hunger") == 100f)
        //{
        //    EarnAchievement("Eating");
        //}
        if (PlayerPrefs.GetFloat("Happiness") == 100f)
        {
            EarnAchievement("Счастливое детство");
        }

    }

    public void EarnAchievement(string title)
    {
        if (achievements[title].EarnAchievement())
        {
            //DO SMTH AWESOME
            GameObject achievement = (GameObject)Instantiate(visualAchievement);
            SetAchievementInfo("EarnCanvas", achievement, title);
            textPoints.text = "";
            StartCoroutine(FadeAchievement(achievement));

            if (achievements[title].BronzeProgression != 0)
            {

                if (achievements[title].MaxProgression == achievements[title].BronzeProgression)
                {
                    PlayerPrefs.DeleteKey(title);
                    achievements[title].Unlocked = false;
                    achievements[title].MaxProgression = achievements[title].SilverProgression;
                    PlayerPrefs.SetInt("MaxProgression" + title, achievements[title].MaxProgression);

                    SetAchievementMaxProgression(achievements[title]);
                }

                else if (achievements[title].MaxProgression == achievements[title].SilverProgression)
                {
                    PlayerPrefs.DeleteKey(title);
                    achievements[title].Unlocked = false;
                    achievements[title].MaxProgression = achievements[title].GoldProgression;
                    PlayerPrefs.SetInt("MaxProgression" + title, achievements[title].MaxProgression);

                    SetAchievementMaxProgression(achievements[title]);
                }

                else if (achievements[title].MaxProgression == achievements[title].GoldProgression)
                {
                    SetAchievementMaxProgression(achievements[title]);
                }

            }

        }
    }

    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);
    }

    public void CreateAchievement(string parent, string title, string description, int points, int spriteIndex, int bronzeProgression, int silverProgression = 0, int goldProgression = 0)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);

        Achievement newAchievement = new Achievement(title, description, points, spriteIndex, achievement, bronzeProgression, silverProgression, goldProgression);

        achievements.Add(title, newAchievement);

        SetAchievementInfo(parent, achievement, title, newAchievement.MaxProgression);
    }

    public void SetAchievementInfo(string parent, GameObject achievement, string title, int progression = 0)
    {
        achievement.transform.SetParent(GameObject.Find(parent).transform, false);

        string progress = progression > 0 ? " " + PlayerPrefs.GetInt("Progression" + title) + "/" + progression : string.Empty;

        Transform titleTransform = achievement.transform.Find("title");
        Transform descriptionTransform = achievement.transform.Find("description");
        Transform coinsTransform = achievement.transform.Find("points");
        Transform pictureTransform = achievement.transform.Find("picture");
        Transform medalTransform = achievement.transform.Find("medal"); 


        if (titleTransform != null)
            titleTransform.GetComponent<TextMeshProUGUI>().text = title + progress;

        if (descriptionTransform != null)
            descriptionTransform.GetComponent<TextMeshProUGUI>().text = achievements[title].Description;

        if (coinsTransform != null)
            coinsTransform.GetComponent<TextMeshProUGUI>().text = achievements[title].Points.ToString();

        if (pictureTransform != null)
            pictureTransform.GetComponent<Image>().sprite = sprites[achievements[title].SpriteIndex];

        // Установка медали

        
        if (medalTransform!= null && achievements[title].BronzeProgression != 0)
        {
            Image medalImage = medalTransform.GetComponent<Image>();
            if (achievements[title].CurrentProgression == achievements[title].GoldProgression)
            {
                medalImage.color = new Color(1, 1, 1, 1);
                medalImage.sprite = goldMedal;
            }
            else if (achievements[title].CurrentProgression == achievements[title].SilverProgression)
            {
                medalImage.color = new Color(1, 1, 1, 1);
                medalImage.sprite = silverMedal;
            }
            else if (achievements[title].CurrentProgression == achievements[title].BronzeProgression)
            {
                medalImage.color = new Color(1, 1, 1, 1);
                medalImage.sprite = bronzeMedal;
            }
            else
            {
                medalImage.color = new Color(1, 1, 1, 0);
            }
            
        }
        else if (medalTransform != null)
        {
            Image medalImage = medalTransform.GetComponent<Image>();
            medalImage.color = new Color(1, 1, 1, 0); // Установить прозрачный цвет
        }

    }

    public void SetAchievementMaxProgression(Achievement achievement)
    {
        achievement.TransformMaxProgression();
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


        for (int i = 0; i < 2; i++)
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
