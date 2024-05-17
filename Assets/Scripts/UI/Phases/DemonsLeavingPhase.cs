﻿using UnityEngine;

public class DemonsLeavingPhase : MonoBehaviour
{
    private void Update()
    {
        for (int i = GameManager.Instance.CurrentCards.Count; i > 0; i--) {
            DraggableCard card = GameManager.Instance.CurrentCards[i];
            if (card.Card.Data.Type != CardData.CardType.Demonic)
            {
                continue;
            }

            if (!card.Card.HasCommitedMurder)
            {
                GameManager.Instance.CurrentCards.Remove(card);
                Destroy(card.gameObject);
            }
        }
        
        GameUI.Instance.MoonPhaseProgress.GameState = MoonPhaseProgress.State.NEW_MOON;
    }

    public void OnStateChanged(MoonPhaseProgress.State state, int night)
    {
        gameObject.SetActive(state == MoonPhaseProgress.State.COMBAT_END);
    }
}