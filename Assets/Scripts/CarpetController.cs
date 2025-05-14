using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetController : MonoBehaviour
{
    public Material material_1; // материал для 1-го ковра
    public Material material_2; // материал для 2-го ковра
    public GameObject Carpet;
    public GameObject Bed;
    void Update()
    {
        int carpet_1 = PlayerPrefs.GetInt("Item_Mat1_Selected", 0);
        int carpet_2 = PlayerPrefs.GetInt("Item_Mat2_Selected", 0);
        int bed = PlayerPrefs.GetInt("Item_Bed_Selected", 0);

        if (carpet_1 == 1)
        {
            SetMaterial(material_1);
            Carpet.SetActive(true);
        }

        else if (carpet_2 == 1)
        {
            SetMaterial(material_2);
            Carpet.SetActive(true);
        }

        else if (bed == 1)
        {
            Bed.SetActive(true);
        }
    }
    void SetMaterial(Material material)
    {
        Renderer renderer = Carpet.GetComponent<Renderer>(); // получаем материал ковра
        renderer.material = material;
    }
}
