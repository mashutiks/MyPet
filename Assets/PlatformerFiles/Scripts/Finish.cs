using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    // ìåòîä âûçûâàåòñÿ, êîãäà îáúåêò âõîäèò â òðèããåð (êîëëàéäåð ñ âêëþ÷¸ííûì IsTrigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ïðîâåðêà, ÷òî â òðèããåð ïîïàë îáúåêò ñ òåãîì "Player"
        if (other.CompareTag("Player"))
        {
            // ïîëó÷èëè èíäåêñ òåêóùåé ñöåíû è çàãðóæàåì ñëåäóþùóþ
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // çàãðóæàåì ñëåäóþùóþ ñöåíó, èñïîëüçóÿ èíäåêñ òåêóùåé ñöåíû + 1
            SceneManager.LoadScene(currentSceneIndex + 1);
            if (currentSceneIndex == 1)
                AchievemenetManager.Instance.EarnAchievement("Прыжок в небеса");
            if (currentSceneIndex == 2)
                AchievemenetManager.Instance.EarnAchievement("Уклонение от пернатых");
            if (currentSceneIndex == 3)
                AchievemenetManager.Instance.EarnAchievement("Кошачьи ловушки");
            if (currentSceneIndex == 4)
                AchievemenetManager.Instance.EarnAchievement("На волоске от победы");
            if (currentSceneIndex == 5)
                AchievemenetManager.Instance.EarnAchievement("Победитель птиц и кошек");

        }
    }
}