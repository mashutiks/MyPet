using UnityEngine;
using UnityEngine.UI;

public class ButtonClickTracker : MonoBehaviour
{
    [Header("���� ��� PlayerPrefs")]
    public string playerPrefsKey;

    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(TrackClick);
        }
        else
        {
            Debug.LogWarning("[SimpleClickTracker] ������ �� ������� �� �������: " + gameObject.name);
        }
    }

    void TrackClick()
    {
        int count = PlayerPrefs.GetInt(playerPrefsKey, 0);
        count++;
        PlayerPrefs.SetInt(playerPrefsKey, count);
        PlayerPrefs.Save();

        Debug.Log($"[SimpleClickTracker] {playerPrefsKey} incremented to {count}");
    }
}
