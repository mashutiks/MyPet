using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void Start()
    {
        // Установка начального состояния
        SetupItem("Item_Food1", 5, food1Button, food1Text);
        SetupItem("Item_Food2", 10, food2Button, food2Text);
        SetupItem("Item_Food3", 15, food3Button, food3Text);
        SetupItem("Item_Stick", 0, stickButton, stickText);
        SetupItem("Item_Fish", 10, fishButton, fishText);
        SetupItem("Item_Bone", 5, boneButton, boneText);
        SetupItem("Item_Mat1", 20, mat1Button, mat1Text);
        SetupItem("Item_Bed", 25, bedButton, bedText);
        SetupItem("Item_Mat2", 20, mat2Button, mat2Text);

        // Привязка кнопок к функциям покупки
        food1Button.onClick.AddListener(() => BuyItem("Item_Food1", 5, food1Button, food1Text));
        food2Button.onClick.AddListener(() => BuyItem("Item_Food2", 10, food2Button, food2Text));
        food3Button.onClick.AddListener(() => BuyItem("Item_Food3", 15, food3Button, food3Text));
        stickButton.onClick.AddListener(() => BuyItem("Item_Stick", 0, stickButton, stickText));
        fishButton.onClick.AddListener(() => BuyItem("Item_Fish", 10, fishButton, fishText));
        boneButton.onClick.AddListener(() => BuyItem("Item_Bone", 5, boneButton, boneText));
        mat1Button.onClick.AddListener(() => BuyItem("Item_Mat1", 20, mat1Button, mat1Text));
        bedButton.onClick.AddListener(() => BuyItem("Item_Bed", 25, bedButton, bedText));
        mat2Button.onClick.AddListener(() => BuyItem("Item_Mat2", 20, mat2Button, mat2Text));
    }

    void SetupItem(string key, int price, Button button, TextMeshProUGUI text)
    {
        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            button.interactable = false;
            text.text = "Куплено";
        }
        else
        {
            button.interactable = true;
            text.text = price > 0 ? $"Купить" : "Получить";
        }
    }

    void BuyItem(string key, int price, Button button, TextMeshProUGUI text)
    {
        int points = PlayerPrefs.GetInt("Points", 0);

        if (PlayerPrefs.GetInt(key, 0) == 1)
            return;

        if (points >= price)
        {
            points -= price;
            PlayerPrefs.SetInt("Points", points);
            PlayerPrefs.SetInt(key, 1);
            button.interactable = false;
            text.text = "Куплено";
        }
        else
        {
            Debug.Log("Недостаточно монет!");
        }
    }
}
