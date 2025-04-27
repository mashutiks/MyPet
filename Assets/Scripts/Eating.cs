using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class Eating : MonoBehaviour
{
    private GameObject player; // собака
    private Transform dog; // положение собаки на сцене
    private Vector3 dog_position; // вектор с координатами собаки на сцене
    private Transform start_dog; // начальное положение собаки на сцене
    private Vector3 start_dog_position; // вектор с начальными координатами собаки на сцене
    public Button EatingButton; // кнопка "Покормить"
    public GameObject EatingBowl; // миска для еды (объект)
    public GameObject DrinkingBowl; // миска для воды
    private Transform eating_bowl; // положение миски для еды на сцене
    private Vector3 eating_bowl_position; // вектор с координатами миски с едой
    private Transform start_eating_bowl; // начальное положение миски для еды на сцене
    private Vector3 start_eating_bowl_position; // вектор с начальными координатами миски с едой
    private Vector3 start_eating_bowl_scale; // начальный размер миски с едой
    private Transform drinking_bowl; // положение миски для воды на сцене
    private Vector3 drinking_bowl_position; // вектор с координатами миски с водой
    private Transform start_drinking_bowl; // начальное положение миски для воды на сцене
    private Vector3 start_drinking_bowl_position; // вектор с начальными координатами миски с водой
    private Vector3 start_drinking_bowl_scale; // начальный размер миски с водой
    private Animator animator; // анимации собаки
    private string dog_id; // переменная с ID собаки
    private float bowl_distance; // расстояние от собаки до миски
    public GameObject big_granule; // большая гранула корма
    public GameObject[] small_granules; // маленькие гранулы корма
    public GameObject water; // вода
    void Start()
    {
        if (EatingButton != null)
        {
            EatingButton.interactable = true; // кнопка активна
            EatingButton.onClick.AddListener(Feed);
        }
        else
        {
            Debug.LogWarning("Кнопка 'Покормить' не назначена!");
        }
    }

    private void Feed()
    {
        EatingButton.interactable = false; // блокируем нажатие на кнопку во время механики
        small_granules = GameObject.FindGameObjectsWithTag("SmallGranule"); // сохраняем все маленькие гранулы корма
        dog_id = PlayerPrefs.GetString("SelectedDogID", ""); // узнаём, какая у нас собака
        // В зависимости от вида собаки устанавливаем расстояние до миски
        if (dog_id == "germanshepherd")
        {
            bowl_distance = 0.4f;
        }
        else if (dog_id == "cur" || dog_id == "corgi")
        {
            bowl_distance = 0.3f;
        }
        else if (dog_id == "pug")
        {
            bowl_distance = 0.2f;
        }

        player = GameObject.FindGameObjectWithTag("Player"); // получили объект собаки по тегу
        dog = player.transform; // получили положение объекта собаки
        start_dog = dog; // запомнили начальное положение собаки
        dog_position = dog.position; // сохранили координаты собаки
        start_dog_position = dog_position; // запомнили начальные координаты собаки

        eating_bowl = EatingBowl.transform; // получили положение миски с едой
        start_eating_bowl = eating_bowl; // запоминаем начальное положение миски с едой
        start_eating_bowl_scale = eating_bowl.localScale; // запоминаем начальный размер миски с едой

        drinking_bowl = DrinkingBowl.transform; // получили положение миски с водой
        start_drinking_bowl = drinking_bowl; // запоминаем начальное положение миски с водой
        start_drinking_bowl_scale = drinking_bowl.localScale; // запоминаем начальный размер миски с водой

        if (dog_id == "chihuahua")
        {
            bowl_distance = 0.15f;
            eating_bowl.localScale *= 0.65f; // уменьшаем размер миски с едой
            drinking_bowl.localScale *= 0.65f; // уменьшаем размер миски с водой
        }

        eating_bowl_position = eating_bowl.position; // сохранили координаты миски
        start_eating_bowl_position = eating_bowl_position; // запоминаем начальные координаты миски с едой
        eating_bowl_position.z = dog_position.z; // миска с едой будет стоять там, где до этого была собака

        drinking_bowl_position = drinking_bowl.position; // сохранили координаты миски с водой
        start_drinking_bowl_position = drinking_bowl_position; // запоминаем начальные координаты миски с водой
        drinking_bowl_position.z = dog_position.z; // миска с водой будет стоять там, где до этого была собака

        dog_position.z -= bowl_distance; // уменьшили по z (отходим назад)
        dog.position = dog_position; // применили к объекту собаки

        eating_bowl.position = eating_bowl_position; // применили к объекту миски с едой

        animator = player.GetComponent<Animator>(); // получаем объект аниматор собаки
        StartCoroutine(Eat()); // запускаем еду
    }

    private IEnumerator Eat()
    {
        animator.SetInteger("AnimationID", 5); // анимация еды (номер 5)
        foreach (GameObject small_granule in small_granules)
        {
            small_granule.GetComponent<Renderer>().enabled = false; // делаем маленькую гранулу корма невидимой
            yield return new WaitForSeconds(0.3f); // ждём несколько секунд перед скрытием следующей
        }

        yield return new WaitForSeconds(0.5f); // ждём немного перед скрытием большой гранулы
        big_granule.GetComponent<Renderer>().enabled = false;
        animator.SetInteger("AnimationID", 0); // анимация дыхания (номер 0)
        yield return new WaitForSeconds(3f);
        eating_bowl.position = start_eating_bowl_position; // отправляем миску с едой на место
        eating_bowl.localScale = start_eating_bowl_scale; // восстанавливаем изначальный размер миски
        ReturnGranules(); // возвращаем еду обратно
    }

    public void ReturnGranules() // снова включить гранулы корма
    {
        foreach (GameObject small_granule in small_granules)
        {
            small_granule.GetComponent<Renderer>().enabled = true; //  делаем маленькую гранулу корма видимой
        }
        big_granule.GetComponent<Renderer>().enabled = true; // делаем большую гранулу видимой
        // Ставим миску с водой
        drinking_bowl.position = drinking_bowl_position; // применили к объекту миски с водой
        StartCoroutine(Drink()); // запускаем воду
    }

    private IEnumerator Drink()
    {
        animator.SetInteger("AnimationID", 5); // анимация еды (номер 5)
        yield return new WaitForSeconds(3f); // ждём немного перед скрытием воды
        water.GetComponent<Renderer>().enabled = false; // скрываем воду
        animator.SetInteger("AnimationID", 0); // анимация дыхания (номер 0)
        yield return new WaitForSeconds(3f);
        drinking_bowl.position = start_drinking_bowl_position; // отправляем миску с водой на место
        drinking_bowl.localScale = start_drinking_bowl_scale; // восстанавливаем изначальный размер миски
        ReturnWater(); // возвращаем воду обратно
    }

    public void ReturnWater() // снова включить воду
    {
        water.GetComponent<Renderer>().enabled = true; // делаем воду видимой
        dog.position = start_dog_position; // ставим собаку в исходную точку
        EatingButton.interactable = true; // снова активируем кнопку
    }
}
