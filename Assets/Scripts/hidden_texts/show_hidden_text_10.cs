using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class show_hidden_text_10 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Panel_10;

    void Start()
    {
        if (Panel_10 != null)
        {
            Panel_10.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData event_data)
    {
        if (Panel_10 != null)
        {
            Panel_10.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        if (Panel_10 != null)
        {
            Panel_10.SetActive(false);
        }
    }
}
