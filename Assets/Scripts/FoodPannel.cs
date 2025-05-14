using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class FoodPannel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;
    public string message;

    public void OnPointerEnter(PointerEventData eventData)
    {
        int food_1 = PlayerPrefs.GetInt("Item_Food1_Selected", 0);
        int food_2 = PlayerPrefs.GetInt("Item_Food2_Selected", 0);
        int food_3 = PlayerPrefs.GetInt("Item_Food3_Selected", 0);

        if (food_1 == 0 && food_2 == 0 && food_3 == 0)
        {
            tooltipPanel.SetActive(true);
            tooltipText.text = message;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPanel.SetActive(false);
    }
}
