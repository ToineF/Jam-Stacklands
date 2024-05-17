using System;
using System.Collections.Generic;
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

    public void HumanAttack(DraggableCard card)
    {
        DraggableCard targetCard = GetBestCardToAttack(out bool isDemon);
        if (isDemon)
        {
            Debug.Log("attack demon");
            targetCard.BattleWithCard(card);
        }
        else
        {
            Debug.Log("attack resource");
            targetCard.DestroyCard();
            card.DestroyCard();
        }
    }


    public DraggableCard GetBestCardToAttack(out bool isDemon)
    {
        isDemon = false;
        foreach (var card in GameManager.Instance.CurrentCards)
        {
            if (card.Card.Data.Type == CardData.CardType.Resource) return card;
        }
        foreach (var card in GameManager.Instance.CurrentCards)
        {
            if (card.Card.Data.Type == CardData.CardType.Satanic) return card;
        }
        foreach (var card in GameManager.Instance.CurrentCards)
        {
            isDemon = true;
            if (card.Card.Data.Type == CardData.CardType.Demonic) return card;
        }
        return GameManager.Instance.CurrentCards[0];
    }
}