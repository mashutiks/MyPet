using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class show_hidden_text_7 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Panel_7;

    void Start()
    {
        if (Panel_7 != null)
        {
            Panel_7.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData event_data)
    {
        if (Panel_7 != null)
        {
            Panel_7.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        if (Panel_7 != null)
        {
            Panel_7.SetActive(false);
        }
    }
}
