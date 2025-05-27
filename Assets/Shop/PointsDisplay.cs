using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PointsDisplay : MonoBehaviour
{
    public TMP_Text pointsText;

    void Start()
    {
        StartCoroutine(UpdatePointsLoop());
    }

    IEnumerator UpdatePointsLoop()
    {
        while (true)
        {
            int points = PlayerPrefs.GetInt("Points", 0);
            pointsText.text = "" + points.ToString();
            yield return new WaitForSeconds(1f);
        }
    }
}
