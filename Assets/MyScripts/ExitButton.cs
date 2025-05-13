using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
