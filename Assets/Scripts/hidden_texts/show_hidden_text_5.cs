using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class show_hidden_text_5 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Panel_5;

    void Start()
    {
        if (Panel_5 != null)
        {
            Panel_5.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData event_data)
    {
        if (Panel_5 != null)
        {
            Panel_5.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        if (Panel_5 != null)
        {
            Panel_5.SetActive(false);
        }
    }
}
