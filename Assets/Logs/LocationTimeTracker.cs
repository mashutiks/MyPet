using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationTimeTracker : MonoBehaviour
{
    private float timeSpent = 0f;
    private string currentScene = "";

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        InvokeRepeating(nameof(TrackTime), 1f, 1f);
    }
    private float debugLogTimer = 0f;


    void TrackTime()
    {
        string key = GetPlayerPrefsKeyForScene(currentScene);
        if (!string.IsNullOrEmpty(key))
        {
            int previousTime = PlayerPrefs.GetInt(key, 0);
            PlayerPrefs.SetInt(key, previousTime + 1);
            PlayerPrefs.Save();
            Debug.Log($"[LOG] Time tracked in {currentScene}: {previousTime + 1} sec");
        }
    }

    string GetPlayerPrefsKeyForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Home": return "Time_Home_Seconds";
            case "Park": return "Time_Park_Seconds";
            case "Playground": return "Time_Playground_Seconds";
            case "Training_zone": return "Time_Training_Seconds";
            case "Shop": return "Time_Shop_Seconds";
            case "GameOver":
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
                return "Time_MiniGame_Seconds";

            default: return null;
        }
    }
}
