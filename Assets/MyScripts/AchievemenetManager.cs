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

    private AudioSource notificationSound;


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

    void Awake()
    {
        notificationSound = GetComponent<AudioSource>();
    }

    void Start()
    {
        textPoints.text = "";


        activeButton = GameObject.Find("generalButton").GetComponent<AchievementCategoryButtons>();

        CreateAchievement("general", "На свежем воздухе", "Прогулять питомца вдоволь", 5, 1, 0);
        CreateAchievement("general", "Счастливое детство", "Достичь максимального счастья питомца", 15, 2, 0);
        CreateAchievement("general", "Все, кроме голодовки", "Накормить питомца", 10, 0, 1, 3, 5);
        CreateAchievement("general", "Делу - время", "Заняться дрессировкой", 10, 5, 1, 7, 20);

        CreateAchievement("shop", "Шопинг - наше все", "Купить в магазине что-нибудь", 10, 7, 1, 8, 20);
        CreateAchievement("shop", "Ммм... Вкуснятина", "Купить новый корм", 5, 8, 1, 2, 3);
        CreateAchievement("shop", "Ура! У нас новая игрушка", "Купить новую игрушку", 5, 10, 1, 2, 3);
        CreateAchievement("shop", "Тепло и уютно", "Купить что-то в дом", 5, 9, 1, 2, 3);


        CreateAchievement("miniGames", "Прыжок в небеса", "Пройти первый уровень", 5, 3, 0);
        CreateAchievement("miniGames", "Уклонение от пернатых", "Допрыгать второй", 10, 12, 0);
        CreateAchievement("miniGames", "Кошачьи ловушки", "Преодолеть три уровня", 15, 13, 0);
        CreateAchievement("miniGames", "На волоске от победы", "Справиться с четыремя", 20, 14, 0);
        CreateAchievement("miniGames", "Победитель птиц и кошек", "Пройти все пять уровней", 25, 15, 0);

        CreateAchievement("others", "Лучше 100 друзей", "Познакомиться с другом", 10, 4, 1, 5, 10);
        CreateAchievement("others", "Лучше 0 врагов", "Поругаться с кем-то", 10, 6, 1, 5, 10);
        CreateAchievement("others", "Секретное достижение", "???", 99, 11, 0);

        CreateAchievement("activity", "Заботливый хозяин", "Зайти в игру 2 дня подряд", 5, 16, 0);
        CreateAchievement("activity", "Верный друг", "Зайти в игру 5 дней подряд", 10, 17, 0);
        CreateAchievement("activity", "Собачий исследователь", "Зайти в игру 7 дней подряд", 15, 18, 0);
        CreateAchievement("activity", "Настоящий питомцевод", "Зайти в игру 14 дней подряд", 30, 19, 0);
        CreateAchievement("activity", "Лучший хозяин", "Зайти в игру 21 день подряд", 50, 20, 0);
        CreateAchievement("activity", "Собачья легенда", "Зайти в игру 30 дней подряд", 99, 21, 0);



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
            if (achievements[title].CurrentProgression >= achievements[title].GoldProgression)
            {
                medalImage.color = new Color(1, 1, 1, 1);
                medalImage.sprite = goldMedal;
            }
            else if (achievements[title].CurrentProgression >= achievements[title].SilverProgression)
            {
                medalImage.color = new Color(1, 1, 1, 1);
                medalImage.sprite = silverMedal;
            }
            else if (achievements[title].CurrentProgression >= achievements[title].BronzeProgression)
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

        notificationSound.Play();

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
