using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed = 5f; // �������� �������� �����

    void Update()
    {
        // ������� ����� � �������� �����������
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // ������� �����, ���� ��� ������� �� ������� ������
        if (transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
