using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StatsExporter : MonoBehaviour
{
    public Button exportButton;

    private List<string> keys = new List<string>()
    {
        "Location_Home_Clicks",
        "Location_Park_Clicks",
        "Location_Playground_Clicks",
        "Location_Training_Clicks",
        "Location_Shop_Clicks",
        "Location_MiniGame_Clicks",
        "Location_Achievements_Clicks",

        "Dog_Pug_Chosen",
        "Dog_Germanshepherd_Chosen",
        "Dog_Cur_Chosen",
        "Dog_Corgi_Chosen",
        "Dog_Chihuahua_Chosen",

        "Toy_Stick_Bought",
        "Toy_Bone_Bought",
        "Toy_Fish_Bought",

        "Furniture_RugA_Bought",
        "Furniture_RugB_Bought",
        "Furniture_Bed_Bought",

        "Food_Red_Bought",
        "Food_Green_Bought",
        "Food_Blue_Bought",

        "Time_Home_Seconds",
        "Time_Park_Seconds",
        "Time_Playground_Seconds",
        "Time_Training_Seconds",
        "Time_Shop_Seconds",
        "Time_MiniGame_Seconds",

        "MiniGame_Level1_Completed",
        "MiniGame_Level1_Deaths",
        "MiniGame_Level2_Completed",
        "MiniGame_Level2_Deaths",
        "MiniGame_Level3_Completed",
        "MiniGame_Level3_Deaths",
        "MiniGame_Level4_Completed",
        "MiniGame_Level4_Deaths",
        "MiniGame_Level5_Completed",
        "MiniGame_Level5_Deaths",
    };

    void Start()
    {
        if (exportButton != null)
        {
            exportButton.onClick.AddListener(ExportStatsToDesktop);
        }
        else
        {
            Debug.LogWarning("Кнопка для экспорта статистики не назначена!");
        }
    }

    void ExportStatsToDesktop()
    {
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, "MyGameStats.txt");

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (string key in keys)
        {
            int value = PlayerPrefs.GetInt(key, 0);
            sb.AppendLine($"{key}: {value}");
        }

        try
        {
            File.WriteAllText(filePath, sb.ToString());
            Debug.Log($"Статистика сохранена на рабочем столе: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Ошибка при сохранении файла: " + e.Message);
        }
    }
}
