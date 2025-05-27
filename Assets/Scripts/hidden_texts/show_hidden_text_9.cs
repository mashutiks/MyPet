using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class show_hidden_text_9 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Panel_9;

    void Start()
    {
        if (Panel_9 != null)
        {
            Panel_9.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData event_data)
    {
        if (Panel_9 != null)
        {
            Panel_9.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData event_data)
    {
        if (Panel_9 != null)
        {
            Panel_9.SetActive(false);
        }
    }
}
