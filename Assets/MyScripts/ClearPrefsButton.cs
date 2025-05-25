using UnityEngine;

public class ClearPrefsButton : MonoBehaviour
{
    public void ClearAllPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs очищены.");
    }
}
