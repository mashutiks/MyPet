using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;


public class FoodPannel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;
    public string message;

    void Start()
    {
        StartCoroutine(LogFoodStatusEverySecond());
    }
    
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

    IEnumerator LogFoodStatusEverySecond()
    {
        while (true)
        {
            int food_1 = PlayerPrefs.GetInt("Item_Food1_Selected", 0);
            int food_2 = PlayerPrefs.GetInt("Item_Food2_Selected", 0);
            int food_3 = PlayerPrefs.GetInt("Item_Food3_Selected", 0);

            Debug.Log($"Food status -> Food1: {food_1}, Food2: {food_2}, Food3: {food_3}");

            yield return new WaitForSeconds(1f); // ждать 1 секунду
        }
    }






}
