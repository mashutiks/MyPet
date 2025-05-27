using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class show_hidden_text_8 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Panel_8;

    void Start()
    {
        if (Panel_8 != null)
        {
            Panel_8.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData event_data)
    {
        if (Panel_8 != null)
        {
            Panel_8.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        if (Panel_8 != null)
        {
            Panel_8.SetActive(false);
        }
    }
}
