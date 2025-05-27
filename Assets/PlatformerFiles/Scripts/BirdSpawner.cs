using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab; // ������ �����
    public float spawnInterval = 10f; // �������� ��������� �����

    private void Start()
    {
        InvokeRepeating("SpawnBird", 0f, spawnInterval); // ��������� ����� SpawnBird ������ 10 ������
    }

    void SpawnBird()
    {
        // ������� ����� � ��������� ������� �� ���������
        Vector3 spawnPosition = new Vector3(50f, Random.Range(-3f, 1f), 0f);
        GameObject bird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
    }
}