using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Booster : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BoosterData _data;

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < _data.NumberToSpawn; i++)
        {
            Card newCard = Instantiate(GetRandomCard(), transform.position, Quaternion.identity, transform.parent);
            Vector3 randomTargetDirection = newCard.transform.position + (Vector3)Random.insideUnitCircle.normalized * GameManager.Instance.VisualData.CardSpawnDistance;
            newCard.transform.DOMove(randomTargetDirection, GameManager.Instance.VisualData.CardSpawnTime);
        }
        Destroy(gameObject);
    }

    private Card GetRandomCard()
    {
        int total = 0;
        Dictionary<int, Card> cardsToCheck = new Dictionary<int, Card>();
        foreach (var card in _data.SpawnableCards)
        {
            cardsToCheck.Add(total, card.Card);
            total += card.Probability;
        }

        int randomIndex = Random.Range(0, total);
        int max = 0;

        foreach (var card in cardsToCheck)
        {
            if (card.Key > randomIndex) continue;
            if (max < card.Key) max = card.Key;
        }

        return cardsToCheck[max];

    }
}