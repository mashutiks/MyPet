using UnityEngine;

public class DogChoiceTracker : MonoBehaviour
{
    void Start()
    {
        // уже логировался выбор в этой сессии?
        if (PlayerPrefs.GetInt("Dog_Choice_Logged", 0) == 1)
            return;

        string dogID = PlayerPrefs.GetString("SelectedDogID", "");

        string key = GetDogChoiceKey(dogID);
        if (!string.IsNullOrEmpty(key))
        {
            int count = PlayerPrefs.GetInt(key, 0);
            PlayerPrefs.SetInt(key, count + 1);
            PlayerPrefs.SetInt("Dog_Choice_Logged", 1); // отмечаемь что записали
            PlayerPrefs.Save();

            Debug.Log($"[DogChoiceTracker] {dogID} выбран. Ключ: {key}, новое значение: {count + 1}");
        }
        else
        {
            Debug.LogWarning($"[DogChoiceTracker] Неизвестный dogID: {dogID}");
        }
    }

    string GetDogChoiceKey(string dogID)
    {
        switch (dogID)
        {
            case "pug": return "Dog_Pug_Chosen";
            case "germanshepherd": return "Dog_Germanshepherd_Chosen";
            case "cur": return "Dog_Cur_Chosen";
            case "corgi": return "Dog_Corgi_Chosen";
            case "chihuahua": return "Dog_Chihuahua_Chosen";
            default: return null;
        }
    }
}
