using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab; // Префаб птицы
    public float spawnInterval = 10f; // Интервал появления птицы

    private void Start()
    {
        InvokeRepeating("SpawnBird", 0f, spawnInterval); // Запускаем метод SpawnBird каждые 10 секунд
    }

    void SpawnBird()
    {
        // Создаем птицу в случайной позиции по вертикали
        Vector3 spawnPosition = new Vector3(50f, Random.Range(-3f, 1f), 0f);
        GameObject bird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
    }
}