using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public GameObject PauseScreen;
    public MoonPhaseProgress MoonPhaseProgress;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnPauseStateChanged += OnPause;
    }

    private void OnPause(bool isPaused)
    {
        PauseScreen.SetActive(isPaused);
    }
}
