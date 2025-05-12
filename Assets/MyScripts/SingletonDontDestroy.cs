using UnityEngine;

public class SingletonDontDestroy : MonoBehaviour
{
    private void Awake()
    {
        // Проверяем, есть ли уже объект с таким именем в DontDestroyOnLoad
        GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in objs)
        {
            if (obj != this.gameObject && obj.name == gameObject.name)
            {
                Destroy(gameObject); // Удаляем дубликат
                return;
            }
        }

        DontDestroyOnLoad(gameObject);
    }
}