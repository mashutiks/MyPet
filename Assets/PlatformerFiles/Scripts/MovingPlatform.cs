using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; // �������� �������� ���������
    public float moveDistance = 5f; // �����, �� ������� ��������� ������ ��������� ����� � ������

    private Vector2 startPosition; // ��������� ������� ���������
    private int direction = 1; // ����������� �������� (1 = ������, -1 = �����)
    private Transform player; // ������ �� ������
    private bool playerOnPlatform = false; // ��������, ��������� �� ����� �� ���������

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float distanceFromStart = Vector2.Distance(startPosition, transform.position);

        if (distanceFromStart >= moveDistance)
        {
            direction *= -1;
        }

        // ���������� ���������
        Vector2 movement = Vector2.right * direction * speed * Time.deltaTime;
        transform.Translate(movement);

        // ���� ����� �� ���������, ����������� ��� ������ � ����������
        if (playerOnPlatform && player != null)
        {
            // ��������� �������� ��������� � ������� ������
            player.position += new Vector3(movement.x, 0, 0);
        }
    }
    // ������ ������� ���������� ������� � ������ ��������
    void OnCollisionEnter2D(Collision2D collision)
    {
        // ����� �� ������, � ������� ��������� �������, ��� "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // ���������� ������ �� ������ ������, ����� ����� ���� �������� � ���
            player = collision.transform;
            // ����� ��������� �� ��������� - ������
            playerOnPlatform = true;
        }
    }
    // ������ �������� ������� � ������ ��������
    void OnCollisionExit2D(Collision2D collision)
    {
        // ������, � ������� �������� �������, ����� ��� "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // ����� ������ �� �� ���������
            playerOnPlatform = false;
            // ���������� ������ �� ������ ������
            player = null;
        }
    }
}
