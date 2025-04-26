using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip coinClip;
    public AudioClip scanClip;
    private AudioSource src;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            src = gameObject.AddComponent<AudioSource>();
        }
        else Destroy(gameObject);
    }

    public void PlayCoin() => src.PlayOneShot(coinClip);
    public void PlayScan() => src.PlayOneShot(scanClip);
}
