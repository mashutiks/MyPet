using UnityEngine;

public class AchievementsBackButton : MonoBehaviour
{
    public void CloseAchievements()
    {
        var menu = AchievementCanvas.Instance.achievementMenu;

        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
        }
    }
}