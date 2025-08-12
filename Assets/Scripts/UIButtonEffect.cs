using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public bool isStartButton = false; // Neu la nut Start Game thi set true
    public string gameSceneName = "GameScene"; // Ten scene choi game

    public bool isMusicToggle = false; // Neu la nut bat/tat nhac thi set true

    private Vector3 originalScale; // Luu lai scale ban dau
    public float pressScale = 0.9f; // Ti le thu nho khi nhan
    public float returnSpeed = 10f; // Toc do quay ve ban dau

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Khi bam xuong thi thu nho nut
        transform.localScale = originalScale * pressScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Khi nha tay thi quay ve scale cu
        transform.localScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Phat am thanh click
        AudioManager.instance.PlayClick();

        // Xu ly chuc nang nut
        if (isStartButton)
        {
            SceneManager.LoadScene(gameSceneName); // Chuyen sang scene choi game
        }
        else if (isMusicToggle)
        {
            AudioManager.instance.ToggleBGM(); // Bat tat nhac nen
        }
    }
}
