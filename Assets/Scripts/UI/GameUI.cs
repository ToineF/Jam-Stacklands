using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public GameObject PauseScreen;
    
    [Header("Moon Stuff")]
    public MoonPhaseProgress MoonPhaseProgress;
    public WaitingCraftPhase WaitingCraftPhase;
    public HumansAttackingPhase HumansAttackingPhase;
    public HumansSpawningPhase HumansSpawningPhase;
    public DemonsLeavingPhase DemonsLeavingPhase;
    public NewMoonPhase NewMoonPhase;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnPauseStateChanged += OnPause;
        MoonPhaseProgress.OnStateChanged += WaitingCraftPhase.OnStateChanged;
        MoonPhaseProgress.OnStateChanged += HumansAttackingPhase.OnStateChanged;
        MoonPhaseProgress.OnStateChanged += HumansSpawningPhase.OnStateChanged;
        MoonPhaseProgress.OnStateChanged += DemonsLeavingPhase.OnStateChanged;
        MoonPhaseProgress.OnStateChanged += NewMoonPhase.OnStateChanged;
    }

    private void OnPause(bool isPaused)
    {
        PauseScreen.SetActive(isPaused);
    }
}
