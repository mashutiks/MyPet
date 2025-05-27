using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBarkManager : MonoBehaviour
{
    public List<AudioSource> dogAudioSources; 

    private void Start()
    {
        StartCoroutine(BarkRoutine());
    }

    IEnumerator BarkRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(15f, 25f);
            yield return new WaitForSeconds(waitTime);

            int randomDogIndex = Random.Range(0, dogAudioSources.Count);
            AudioSource chosenDog = dogAudioSources[randomDogIndex];

            float randomVolume = Random.Range(0.1f, 1.0f); // 🔊 Рандомная громкость
            chosenDog.volume = randomVolume;

            chosenDog.Play();
        }
    }
}
