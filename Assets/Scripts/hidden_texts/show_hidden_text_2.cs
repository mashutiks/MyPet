using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class show_hidden_text_2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Panel_2;

    void Start()
    {
        if (Panel_2 != null)
        {
            Panel_2.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData event_data)
    {
        if (Panel_2 != null)
        {
            Panel_2.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        if (Panel_2 != null)
        {
            Panel_2.SetActive(false);
        }
    }
}
