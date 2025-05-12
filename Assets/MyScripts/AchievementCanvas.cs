using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCanvas : MonoBehaviour
{
    private static AchievementCanvas instance;
    public GameObject achievementMenu;


    public static AchievementCanvas Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindAnyObjectByType<AchievementCanvas>();
            }
            return AchievementCanvas.instance;
        }
    }
}
