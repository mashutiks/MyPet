using System.Collections;
using UnityEngine;

public class DogWhining : MonoBehaviour
{
    public AudioClip whineSound; 
    private AudioSource audioSource; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); 

        StartCoroutine(CheckDogStatus());
    }

    IEnumerator CheckDogStatus()
    {
        while (true)
        {
            float hunger = PlayerPrefs.GetFloat("Hunger", 0f);
            float energy = PlayerPrefs.GetFloat("Walk", 0f);
            float mood = PlayerPrefs.GetFloat("Happiness", 0f);

            if (hunger == 0 && energy == 0 && mood == 0)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(whineSound);
                }
            }

            yield return new WaitForSeconds(15f);
        }
    }
}