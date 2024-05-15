using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public MoonPhaseProgress MoonPhaseProgress;

    private void Awake()
    {
        Instance = this;
    }
}
