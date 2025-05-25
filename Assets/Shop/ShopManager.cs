using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;


public class ShopManager : MonoBehaviour
{
    public Button food1Button;
    public Button food2Button;
    public Button food3Button;
    public Button stickButton;
    public Button fishButton;
    public Button boneButton;
    public Button mat1Button;
    public Button bedButton;
    public Button mat2Button;

    public TextMeshProUGUI food1Text;
    public TextMeshProUGUI food2Text;
    public TextMeshProUGUI food3Text;
    public TextMeshProUGUI stickText;
    public TextMeshProUGUI fishText;
    public TextMeshProUGUI boneText;
    public TextMeshProUGUI mat1Text;
    public TextMeshProUGUI bedText;
    public TextMeshProUGUI mat2Text;

    public GameObject insufficientFundsPanel;


    void Start()
    {
        SetupItem("Item_Food1", 5, food1Button, food1Text);
        SetupItem("Item_Food2", 10, food2Button, food2Text);
        SetupItem("Item_Food3", 15, food3Button, food3Text);
        SetupItem("Item_Stick", 0, stickButton, stickText);
        SetupItem("Item_Fish", 10, fishButton, fishText);
        SetupItem("Item_Bone", 5, boneButton, boneText);
        SetupItem("Item_Mat1", 20, mat1Button, mat1Text);
        SetupItem("Item_Bed", 25, bedButton, bedText);
        SetupItem("Item_Mat2", 20, mat2Button, mat2Text);

        food1Button.onClick.AddListener(() => BuyOrSelectItem("Item_Food1", 5, food1Text));
        food2Button.onClick.AddListener(() => BuyOrSelectItem("Item_Food2", 10, food2Text));
        food3Button.onClick.AddListener(() => BuyOrSelectItem("Item_Food3", 15, food3Text));
        stickButton.onClick.AddListener(() => BuyOrSelectItem("Item_Stick", 0, stickText));
        fishButton.onClick.AddListener(() => BuyOrSelectItem("Item_Fish", 10, fishText));
        boneButton.onClick.AddListener(() => BuyOrSelectItem("Item_Bone", 5, boneText));
        mat1Button.onClick.AddListener(() => BuyOrSelectItem("Item_Mat1", 20, mat1Text));
        bedButton.onClick.AddListener(() => BuyOrSelectItem("Item_Bed", 25, bedText));
        mat2Button.onClick.AddListener(() => BuyOrSelectItem("Item_Mat2", 20, mat2Text));

    }

    void SetupItem(string key, int price, Button button, TextMeshProUGUI text)
    {
        bool isBought = PlayerPrefs.GetInt(key, 0) == 1;
        bool isSelected = PlayerPrefs.GetInt(key + "_Selected", 0) == 1;

        button.interactable = true;

        if (!isBought)
        {
            text.text = price > 0 ? "Купить" : "Получить";
        }
        else
        {
            text.text = isSelected ? "Выбран" : "Выбрать";
        }
    }

    void BuyOrSelectItem(string key, int price, TextMeshProUGUI text)
    {
        string statKey = GetClickStatKey(key); //ñáîðêà ëîãîâ
        if (!string.IsNullOrEmpty(statKey))
        {
            int currentCount = PlayerPrefs.GetInt(statKey, 0);
            PlayerPrefs.SetInt(statKey, currentCount + 1);
        }

        int points = PlayerPrefs.GetInt("Points", 0);
        bool isBought = PlayerPrefs.GetInt(key, 0) == 1;


        if (isBought)
        {
            string category = GetCategoryFromKey(key);
            SelectItem(key, category);
            UpdateAllButtons();
            return;
        }

        if (points >= price)
        {
            points -= price;
            PlayerPrefs.SetInt("Points", points);
            PlayerPrefs.SetInt(key, 1);

            AchievemenetManager.Instance.EarnAchievement("Шопинг - наше все");

            if (!PlayerPrefs.HasKey(key + "already"))
            {
                if (key == "Item_Food1")
                {
                    AchievemenetManager.Instance.EarnAchievement("Ммм... Вкуснятина");
                    PlayerPrefs.SetInt(key + "already", 1);
                }
                else if (key == "Item_Food2")
                {
                    AchievemenetManager.Instance.EarnAchievement("Ммм... Вкуснятина");
                    PlayerPrefs.SetInt(key + "already", 1);
                }
                else if (key == "Item_Food3")
                {
                    AchievemenetManager.Instance.EarnAchievement("Ммм... Вкуснятина");
                    PlayerPrefs.SetInt(key + "already", 1);
                }
            }

            if (key == "Item_Stick")
            {
                AchievemenetManager.Instance.EarnAchievement("Ура! У нас новая игрушка");
            }
            else if (key == "Item_Fish")
            {
                AchievemenetManager.Instance.EarnAchievement("Ура! У нас новая игрушка");
            }
            else if (key == "Item_Bone")
            {
                AchievemenetManager.Instance.EarnAchievement("Ура! У нас новая игрушка");
            }

            if (key == "Item_Mat1")
            {
                AchievemenetManager.Instance.EarnAchievement("Тепло и уютно");
            }
            else if (key == "Item_Bed")
            {
                AchievemenetManager.Instance.EarnAchievement("Тепло и уютно");
            }
            else if (key == "Item_Mat2")
            {
                AchievemenetManager.Instance.EarnAchievement("Тепло и уютно");
            }

            string category = GetCategoryFromKey(key);
            SelectItem(key, category);
            UpdateAllButtons();
        }
        else
        {
            Debug.Log("Недостаточно монет!");
            StartCoroutine(ShowInsufficientFundsMessage());
        }
    }

    void SelectItem(string key, string category)
    {
        foreach (string itemKey in GetItemsInCategory(category))
        {
            PlayerPrefs.SetInt(itemKey + "_Selected", 0);
        }

        PlayerPrefs.SetInt(key + "_Selected", 1);
        Debug.Log("Выбран предмет: " + key);
    }

    void UpdateAllButtons()
    {
        SetupItem("Item_Food1", 15, food1Button, food1Text);
        SetupItem("Item_Food2", 20, food2Button, food2Text);
        SetupItem("Item_Food3", 25, food3Button, food3Text);
        SetupItem("Item_Stick", 0, stickButton, stickText);
        SetupItem("Item_Fish", 20, fishButton, fishText);
        SetupItem("Item_Bone", 15, boneButton, boneText);
        SetupItem("Item_Mat1", 30, mat1Button, mat1Text);
        SetupItem("Item_Bed", 35, bedButton, bedText);
        SetupItem("Item_Mat2", 30, mat2Button, mat2Text);
    }

    string GetCategoryFromKey(string key)
    {
        if (key.StartsWith("Item_Food")) return "Food";
        if (key == "Item_Stick" || key == "Item_Fish" || key == "Item_Bone") return "Toy";
        if (key.StartsWith("Item_Mat") || key == "Item_Bed") return "Interior";
        return "";
    }

    List<string> GetItemsInCategory(string category)
    {
        switch (category)
        {
            case "Food":
                return new List<string> { "Item_Food1", "Item_Food2", "Item_Food3" };
            case "Toy":
                return new List<string> { "Item_Stick", "Item_Fish", "Item_Bone" };
            case "Interior":
                return new List<string> { "Item_Mat1", "Item_Bed", "Item_Mat2" };
            default:
                return new List<string>();
        }
    }
    IEnumerator ShowInsufficientFundsMessage()
    {
        insufficientFundsPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        insufficientFundsPanel.SetActive(false);
    }

    string GetClickStatKey(string itemKey)
    {
        switch (itemKey)
        {
            case "Item_Stick": return "Toy_Stick_Bought";
            case "Item_Bone": return "Toy_Bone_Bought";
            case "Item_Fish": return "Toy_Fish_Bought";
            case "Item_Mat1": return "Furniture_RugA_Bought";
            case "Item_Mat2": return "Furniture_RugB_Bought";
            case "Item_Bed": return "Furniture_Bed_Bought";
            case "Item_Food1": return "Food_Red_Bought";
            case "Item_Food2": return "Food_Green_Bought";
            case "Item_Food3": return "Food_Blue_Bought";
            default: return null;
        }
    }




}