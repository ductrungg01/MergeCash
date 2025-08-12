using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton de su dung o nhieu scene

    public AudioSource bgmSource; // AudioSource cho nhac nen
    public AudioSource sfxSource; // AudioSource cho sound effect

    public AudioClip bgmClip; // File nhac nen
    public AudioClip clickSFX; // Am thanh khi click

    private bool isBGMOn = true; // Trang thai nhac nen

    void Awake()
    {
        // Dam bao chi co 1 AudioManager ton tai
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Khong bi xoa khi doi scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Phat nhac nen luc bat dau
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlayClick()
    {
        // Phat am thanh click
        if (clickSFX != null)
            sfxSource.PlayOneShot(clickSFX);
    }

    public void ToggleBGM()
    {
        // Bat tat nhac nen
        isBGMOn = !isBGMOn;
        if (isBGMOn)
            bgmSource.Play();
        else
            bgmSource.Pause();
    }
}
