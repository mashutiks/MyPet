using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class show_hidden_text_4 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Panel_4;

    void Start()
    {
        if (Panel_4 != null)
        {
            Panel_4.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData event_data)
    {
        if (Panel_4 != null)
        {
            Panel_4.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        if (Panel_4 != null)
        {
            Panel_4.SetActive(false);
        }
    }
}
