using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public AudioClip collectSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int currentPoints = PlayerPrefs.GetInt("Points", 0);
            PlayerPrefs.SetInt("Points", currentPoints + coinValue);

            int miniGamePoints = PlayerPrefs.GetInt("PointsMiniGame", 0);
            PlayerPrefs.SetInt("PointsMiniGame", miniGamePoints + coinValue);

            PlayerPrefs.Save();

            if (collectSound != null)
            {
                GameObject tempAudio = new GameObject("CoinSound");
                AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
                tempSource.clip = collectSound;
                tempSource.Play();
                Destroy(tempAudio, collectSound.length);
            }

            Destroy(gameObject);
        }
    }
}
