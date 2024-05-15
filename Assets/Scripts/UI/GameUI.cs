using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public GameObject PauseScreen;
    
    [Header("Moon Stuff")]
    public MoonPhaseProgress MoonPhaseProgress;
    public EndOfMoonPhase EndOfMoonPhase;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnPauseStateChanged += OnPause;
        MoonPhaseProgress.MoonPhaseOverEvent += EndOfMoonPhase.EndOfMoon;
    }

    private void OnPause(bool isPaused)
    {
        PauseScreen.SetActive(isPaused);
    }
}
