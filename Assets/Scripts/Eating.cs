using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class Eating : MonoBehaviour
{
    private GameObject player; // ������
    private Transform dog; // ��������� ������ �� �����
    private Vector3 dog_position; // ������ � ������������ ������ �� �����
    private Transform start_dog; // ��������� ��������� ������ �� �����
    private Vector3 start_dog_position; // ������ � ���������� ������������ ������ �� �����
    public Button EatingButton; // ������ "���������"
    public GameObject EatingBowl; // ����� ��� ��� (������)
    public GameObject DrinkingBowl; // ����� ��� ����
    private Transform eating_bowl; // ��������� ����� ��� ��� �� �����
    private Vector3 eating_bowl_position; // ������ � ������������ ����� � ����
    private Transform start_eating_bowl; // ��������� ��������� ����� ��� ��� �� �����
    private Vector3 start_eating_bowl_position; // ������ � ���������� ������������ ����� � ����
    private Vector3 start_eating_bowl_scale; // ��������� ������ ����� � ����
    private Transform drinking_bowl; // ��������� ����� ��� ���� �� �����
    private Vector3 drinking_bowl_position; // ������ � ������������ ����� � �����
    private Transform start_drinking_bowl; // ��������� ��������� ����� ��� ���� �� �����
    private Vector3 start_drinking_bowl_position; // ������ � ���������� ������������ ����� � �����
    private Vector3 start_drinking_bowl_scale; // ��������� ������ ����� � �����
    private Animator animator; // �������� ������
    private string dog_id; // ���������� � ID ������
    private float bowl_distance; // ���������� �� ������ �� �����
    public GameObject big_granule; // ������� ������� �����
    public GameObject[] small_granules; // ��������� ������� �����
    public GameObject water; // ����

    public Material material_1; // �������� ��� 1-�� ���� �����
    public Material material_2; // �������� ��� 2-�� ���� �����
    public Material material_3; // �������� ��� 3-�� ���� �����

    void Start()
    {
        if (EatingButton != null)
        {
            CheckItemAvailability();
            //EatingButton.interactable = true; // ������ �������
            EatingButton.onClick.AddListener(Feed);
  
        }
        else
        {
            Debug.LogWarning("������ '���������' �� ���������!");
        }
        

    }

    private void Feed()
    {
        UIBlocker.BlockAllButtons(14f);
        EatingButton.interactable = false; // ��������� ������� �� ������ �� ����� ��������
        small_granules = GameObject.FindGameObjectsWithTag("SmallGranule"); // ��������� ��� ��������� ������� �����
        dog_id = PlayerPrefs.GetString("SelectedDogID", ""); // �����, ����� � ��� ������
        // � ����������� �� ���� ������ ������������� ���������� �� �����
        if (dog_id == "germanshepherd")
        {
            bowl_distance = 0.41f;
        }
        else if (dog_id == "cur" || dog_id == "corgi")
        {
            bowl_distance = 0.3f;
        }
        else if (dog_id == "pug")
        {
            bowl_distance = 0.22f;
        }

        player = GameObject.FindGameObjectWithTag("Player"); // �������� ������ ������ �� ����
        dog = player.transform; // �������� ��������� ������� ������
        start_dog = dog; // ��������� ��������� ��������� ������
        dog_position = dog.position; // ��������� ���������� ������
        start_dog_position = dog_position; // ��������� ��������� ���������� ������

        eating_bowl = EatingBowl.transform; // �������� ��������� ����� � ����
        start_eating_bowl = eating_bowl; // ���������� ��������� ��������� ����� � ����
        start_eating_bowl_scale = eating_bowl.localScale; // ���������� ��������� ������ ����� � ����

        drinking_bowl = DrinkingBowl.transform; // �������� ��������� ����� � �����
        start_drinking_bowl = drinking_bowl; // ���������� ��������� ��������� ����� � �����
        start_drinking_bowl_scale = drinking_bowl.localScale; // ���������� ��������� ������ ����� � �����

        if (dog_id == "chihuahua")
        {
            bowl_distance = 0.15f;
            eating_bowl.localScale *= 0.65f; // ��������� ������ ����� � ����
            drinking_bowl.localScale *= 0.65f; // ��������� ������ ����� � �����
        }

        eating_bowl_position = eating_bowl.position; // ��������� ���������� �����
        start_eating_bowl_position = eating_bowl_position; // ���������� ��������� ���������� ����� � ����
        eating_bowl_position.z = dog_position.z; // ����� � ���� ����� ������ ���, ��� �� ����� ���� ������

        drinking_bowl_position = drinking_bowl.position; // ��������� ���������� ����� � �����
        start_drinking_bowl_position = drinking_bowl_position; // ���������� ��������� ���������� ����� � �����
        drinking_bowl_position.z = dog_position.z; // ����� � ����� ����� ������ ���, ��� �� ����� ���� ������

        dog_position.z -= bowl_distance; // ��������� �� z (������� �����)
        dog.position = dog_position; // ��������� � ������� ������

        eating_bowl.position = eating_bowl_position; // ��������� � ������� ����� � ����

        animator = player.GetComponent<Animator>(); // �������� ������ �������� ������
        StartCoroutine(Eat()); // ��������� ���
    }

    private IEnumerator Eat()
    {
        animator.SetInteger("AnimationID", 5); // �������� ��� (����� 5)
        foreach (GameObject small_granule in small_granules)
        {
            small_granule.GetComponent<Renderer>().enabled = false; // ������ ��������� ������� ����� ���������
            yield return new WaitForSeconds(0.3f); // ��� ��������� ������ ����� �������� ���������
        }

        yield return new WaitForSeconds(0.5f); // ��� ������� ����� �������� ������� �������
        big_granule.GetComponent<Renderer>().enabled = false;
        animator.SetInteger("AnimationID", 0); // �������� ������� (����� 0)
        yield return new WaitForSeconds(3f);
        eating_bowl.position = start_eating_bowl_position; // ���������� ����� � ���� �� �����
        eating_bowl.localScale = start_eating_bowl_scale; // ��������������� ����������� ������ �����
        ReturnGranules(); // ���������� ��� �������
    }

    public void ReturnGranules() // ����� �������� ������� �����
    {
        foreach (GameObject small_granule in small_granules)
        {
            small_granule.GetComponent<Renderer>().enabled = true; //  ������ ��������� ������� ����� �������
        }
        big_granule.GetComponent<Renderer>().enabled = true; // ������ ������� ������� �������
        // ������ ����� � �����
        drinking_bowl.position = drinking_bowl_position; // ��������� � ������� ����� � �����
        StartCoroutine(Drink()); // ��������� ����
    }

    private IEnumerator Drink()
    {
        animator.SetInteger("AnimationID", 5); // �������� ��� (����� 5)
        yield return new WaitForSeconds(3f); // ��� ������� ����� �������� ����
        water.GetComponent<Renderer>().enabled = false; // �������� ����
        animator.SetInteger("AnimationID", 0); // �������� ������� (����� 0)
        yield return new WaitForSeconds(3f);
        drinking_bowl.position = start_drinking_bowl_position; // ���������� ����� � ����� �� �����
        drinking_bowl.localScale = start_drinking_bowl_scale; // ��������������� ����������� ������ �����
        ReturnWater(); // ���������� ���� �������
    }

    public void ReturnWater() // ����� �������� ����
    {
        water.GetComponent<Renderer>().enabled = true; // ������ ���� �������
        dog.position = start_dog_position; // ������ ������ � �������� �����
        //EatingButton.interactable = true; // ����� ���������� ������
        ResetFoodFlags();
    }

    void CheckItemAvailability()
    {
        int food_1 = PlayerPrefs.GetInt("Item_Food1_Selected", 0);
        int food_2 = PlayerPrefs.GetInt("Item_Food2_Selected", 0);
        int food_3 = PlayerPrefs.GetInt("Item_Food3_Selected", 0);
        int food_1_choose = PlayerPrefs.GetInt("Item_Food1", 0);
        int food_2_choose = PlayerPrefs.GetInt("Item_Food2", 0);
        int food_3_choose = PlayerPrefs.GetInt("Item_Food3", 0);

        bool hasItem = (food_1 == 1 || food_2 == 1 || food_3 == 1);

        if (EatingButton != null)
        {
            if (!hasItem)
            {
                EatingButton.interactable = false;
                return;
            }

            EatingButton.interactable = true;
        }

        if (food_1 == 1)
        {
            SetMaterial(material_1);
        }
        else if (food_2 == 1)
        {
            SetMaterial(material_2);
        }
        else if (food_3 == 1)
        {
            SetMaterial(material_3);
        }
    }


    void SetMaterial(Material material)
    {
        // ������ �������� � ����������� �� ���������� �����
        Renderer[] child_renderers = EatingBowl.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in child_renderers)
        {
            if (renderer.transform == EatingBowl.transform) // ������� �����
                continue;
            renderer.material = material;
        }
    }
    void ResetFoodFlags()
    {
        PlayerPrefs.SetInt("Item_Food1", 0);
        PlayerPrefs.SetInt("Item_Food1_Selected", 0);

        PlayerPrefs.SetInt("Item_Food2", 0);
        PlayerPrefs.SetInt("Item_Food2_Selected", 0);

        PlayerPrefs.SetInt("Item_Food3", 0);
        PlayerPrefs.SetInt("Item_Food3_Selected", 0);
    }
}


