using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;
    private AudioSource mainSource;
    private AudioSource secondSource;

    [Header("Музыка по сценам")]
    [SerializeField] private AudioClip homeAndShopMusic;
    [SerializeField] private AudioClip parkMusic_1;
    [SerializeField] private AudioClip parkMusic_2;
    [SerializeField] private AudioClip pickPetMusic;
    [SerializeField] private AudioClip playgroundMusic;
    [SerializeField] private AudioClip trainingZoneMusic;

    [Header("Настройки перехода")]
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Звук кнопок")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float clickVolume = 0.7f;

    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        mainSource = gameObject.AddComponent<AudioSource>();
        secondSource = gameObject.AddComponent<AudioSource>();

        mainSource.loop = true;
        secondSource.loop = true;

        mainSource.playOnAwake = false;
        secondSource.playOnAwake = false;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        // Home и Shop: не перезапускать трек, если он уже играет
        if ((sceneName == "Home" || sceneName == "Shop") &&
            mainSource.clip == homeAndShopMusic &&
            mainSource.isPlaying)
        {
            SetupButtonSounds();
            return;
        }

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(SwitchMusicWithFade(sceneName));
    }

    private IEnumerator SwitchMusicWithFade(string sceneName)
    {
        float startVolumeMain = mainSource.volume;
        float startVolumeSecond = secondSource.volume;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float factor = 1 - (t / fadeDuration);
            mainSource.volume = startVolumeMain * factor;
            secondSource.volume = startVolumeSecond * factor;
            yield return null;
        }

        mainSource.Stop();
        secondSource.Stop();
        mainSource.clip = null;
        secondSource.clip = null;
        mainSource.volume = startVolumeMain;
        secondSource.volume = startVolumeSecond;

        if (sceneName == "Home" || sceneName == "Shop")
        {
            mainSource.clip = homeAndShopMusic;
            mainSource.Play();
        }
        else if (sceneName == "Park")
        {
            mainSource.clip = parkMusic_1;
            secondSource.clip = parkMusic_2;
            mainSource.Play();
            secondSource.Play();
        }
        else if (sceneName == "Pick_a_pet")
        {
            mainSource.clip = pickPetMusic;
            mainSource.Play();
        }
        else if (sceneName == "Playground")
        {
            mainSource.clip = playgroundMusic;
            mainSource.Play();
        }
        else if (sceneName == "Training_zone")
        {
            mainSource.clip = trainingZoneMusic;
            mainSource.Play();
        }

        // Fade in
        t = 0f;
        mainSource.volume = 0f;
        secondSource.volume = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float factor = t / fadeDuration;
            mainSource.volume = startVolumeMain * factor;
            secondSource.volume = startVolumeSecond * factor;
            yield return null;
        }

        mainSource.volume = startVolumeMain;
        secondSource.volume = startVolumeSecond;

        SetupButtonSounds();
    }

    private void SetupButtonSounds()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveListener(PlayClickSound);
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    private void PlayClickSound()
    {
        if (clickSound != null)
            mainSource.PlayOneShot(clickSound, clickVolume);
    }
}
