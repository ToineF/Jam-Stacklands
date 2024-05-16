using UnityEngine;

public class HumanHandler : MonoBehaviour
{
    private void Start()
    {
        GameUI.Instance.MoonPhaseProgress.OnStateChanged += MoonPhaseOver;
    }

    private void MoonPhaseOver(MoonPhaseProgress.State state, int night)
    {
        
    }
}