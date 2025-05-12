using UnityEngine;

public class AchievementsButtonHandler : MonoBehaviour
{
    public void OpenAchievements()
    {
        var menu = AchievementCanvas.Instance.achievementMenu;

        if (menu == null)
        {
            Debug.LogWarning("AchievementMenu reference missing!");
            return;
        }

        menu.SetActive(!menu.activeSelf);
    }
}