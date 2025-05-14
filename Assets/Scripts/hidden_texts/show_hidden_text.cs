using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class show_hidden_text : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Panel_1;

    void Start()
    {
        if (Panel_1 != null)
        {
            Panel_1.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData event_data)
    {
        if (Panel_1 != null)
        {
            Panel_1.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        if (Panel_1 != null)
        {
            Panel_1.SetActive(false);
        }
    }
}
