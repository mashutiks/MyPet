using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class show_hidden_text_6 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Panel_6;

    void Start()
    {
        if (Panel_6 != null)
        {
            Panel_6.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData event_data)
    {
        if (Panel_6 != null)
        {
            Panel_6.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        if (Panel_6 != null)
        {
            Panel_6.SetActive(false);
        }
    }
}
