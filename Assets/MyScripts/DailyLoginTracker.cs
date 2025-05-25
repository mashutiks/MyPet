using UnityEngine;
using System;
using System.Collections;

public class DailyLoginTracker : MonoBehaviour
{
    private const string LastLoginKey = "LastLoginDate";
    private const string ConsecutiveDaysKey = "ConsecutivePeriods";

    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f);
        TrackDailyLogin();
    }

    private void TrackDailyLogin()
    {
        DateTime lastLoginDate = GetLastLoginDate();
        DateTime currentDate = DateTime.Now.Date; // Получаем текущую дату без времени

        if (lastLoginDate == DateTime.MinValue)
        {
            // Первый вход
            PlayerPrefs.SetString(LastLoginKey, currentDate.ToString());
            PlayerPrefs.SetInt(ConsecutiveDaysKey, 1);
            Debug.Log("Первый вход. Периоды подряд: 1");
        }
        else
        {
            // Проверяем, если текущая дата отличается от последней на один день
            if ((currentDate - lastLoginDate).Days == 1)
            {
                // Увеличиваем счетчик дней подряд
                int consecutive = PlayerPrefs.GetInt(ConsecutiveDaysKey, 1);
                consecutive++;
                PlayerPrefs.SetInt(ConsecutiveDaysKey, consecutive);
                PlayerPrefs.SetString(LastLoginKey, currentDate.ToString());
                Debug.Log($"Увеличение периодов подряд: {consecutive}");
                if (consecutive == 2)
                {
                    AchievemenetManager.Instance.EarnAchievement("Заботливый хозяин");
                }
                else if (consecutive == 5)
                {
                    AchievemenetManager.Instance.EarnAchievement("Верный друг");
                }
                else if (consecutive == 7)
                {
                    AchievemenetManager.Instance.EarnAchievement("Настоящий питомцевод");
                }
                else if (consecutive == 14)
                {
                    AchievemenetManager.Instance.EarnAchievement("Собачий исследователь");
                }
                else if (consecutive == 21)
                {
                    AchievemenetManager.Instance.EarnAchievement("Заботливый хозяин");
                }
                else if (consecutive == 30)
                {
                    AchievemenetManager.Instance.EarnAchievement("Собачья легенда");
                }
            }
            else if ((currentDate - lastLoginDate).Days < 1)
            {
                Debug.Log("Еще не прошло");
            }
            else if ((currentDate - lastLoginDate).Days > 1)
            {
                Debug.Log("Сбросили последовательность");
                PlayerPrefs.SetInt(ConsecutiveDaysKey, 1); // Сбрасываем счетчик
                PlayerPrefs.SetString(LastLoginKey, currentDate.ToString());
            }
        }

        PlayerPrefs.Save();
    }

    private DateTime GetLastLoginDate()
    {
        string lastLoginString = PlayerPrefs.GetString(LastLoginKey, DateTime.MinValue.ToString());
        DateTime lastLoginDate;

        if (DateTime.TryParse(lastLoginString, out lastLoginDate))
        {
            return lastLoginDate;
        }

        return DateTime.MinValue; 
    }
}
