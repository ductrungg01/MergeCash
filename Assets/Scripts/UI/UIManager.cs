using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    [Header("References")]
    public Canvas canvas;           
    public GameObject loadingPopup;
    public GameObject FailedPopup;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        //DontDestroyOnLoad(gameObject);

        if (!canvas)
        {
            canvas = GetComponent<Canvas>();
            if (!canvas)
            {
                Debug.LogError("UIManager requires a Canvas component!");
            }
        }
    }

    public void ShowLoading(bool show)
    {
        if (!loadingPopup)
        {
            Debug.LogWarning("LoadingPopup is not assigned in UIManager!");
            return;
        }

        loadingPopup.SetActive(show);

        if (show)
        {
            loadingPopup.transform.SetAsLastSibling();
        }
    }

    public void ShowFailedPopup(bool show)
    {
        if (!FailedPopup)
        {
            Debug.LogWarning("FailedPopup is not assigned in UIManager!");
            return;
        }

        FailedPopup.SetActive(show);

        if (show)
        {
            FailedPopup.transform.SetAsLastSibling();
            FailedPopup failedPopup = FailedPopup.GetComponent<FailedPopup>();
            failedPopup.Setup(1200, 10000);
        }
    }
}
