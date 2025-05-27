using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;
    public string message;

    public void OnPointerEnter(PointerEventData eventData)
    {
        int stick = PlayerPrefs.GetInt("Item_Stick_Selected", 0);
        int bone = PlayerPrefs.GetInt("Item_Bone_Selected", 0);
        int fish = PlayerPrefs.GetInt("Item_Fish_Selected", 0);

        if (stick == 0 && bone == 0 && fish == 0)
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
