using UnityEngine;

public class AchievementsUI : MonoBehaviour
{
    public GameObject achievementsPanel;

    public void OpenPanel()
    {
        achievementsPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        achievementsPanel.SetActive(false);
    }
}