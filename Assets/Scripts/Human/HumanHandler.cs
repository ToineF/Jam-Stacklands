using System;
using UnityEngine;

public class HumanHandler : MonoBehaviour
{
    [SerializeField] private HumanSpawnsData[] _humansToSpawn;
    [SerializeField] private int _firstNightID;

    private void Start()
    {
        GameUI.Instance.MoonPhaseProgress.OnStateChanged += MoonPhaseOver;
    }

    private void MoonPhaseOver(MoonPhaseProgress.State state, int night)
    {
        Debug.Log(state);
        if (state == MoonPhaseProgress.State.COMBAT_END)
        {
            SpawnHumans(night);
        }
    }

    private void SpawnHumans(int night)
    {
        if (night < _firstNightID) return;

        night = Mathf.Min(night, _firstNightID + _humansToSpawn.Length);

        CardData.SpawnMultipleCards(_humansToSpawn[night - _firstNightID], transform);
    }
}