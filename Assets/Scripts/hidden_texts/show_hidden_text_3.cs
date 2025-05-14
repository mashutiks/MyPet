using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class show_hidden_text_3 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Panel_3;

    void Start()
    {
        if (Panel_3 != null)
        {
            Panel_3.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData event_data)
    {
        if (Panel_3 != null)
        {
            Panel_3.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        if (Panel_3 != null)
        {
            Panel_3.SetActive(false);
        }
    }
}
