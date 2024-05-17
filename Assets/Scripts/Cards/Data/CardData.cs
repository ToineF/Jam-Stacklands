using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{

    [field: Header("General Params")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public List<CardData> CardsThatCanBeStackedOn { get; private set; }
    [field: SerializeField] public int SellPrice { get; private set; }
    [field: SerializeField] public CardType Type { get; private set; }

    public enum CardType
    {
        Human,
        Demonic,
        Satanic,
        Resource,
        Offering,
    }

    public static CardData GetRandomCard(List<CardProbabilityPair> spawnableCards)
    {
        int total = 0;
        Dictionary<int, CardData> cardsToCheck = new Dictionary<int, CardData>();
        foreach (var card in spawnableCards)
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

    public static void SpawnMultipleCards(CardDataSpawnable cardData, Transform transform)
    {
        for (int i = 0; i < cardData.NumberToSpawn; i++)
        {
            Card newCard = Instantiate(GameManager.Instance.CardPrefab.Card, transform.position, Quaternion.identity, transform.parent);
            newCard.Data = CardData.GetRandomCard(cardData.SpawnableCards);
            newCard.UpdateData();
            Vector3 randomTargetDirection = newCard.transform.position + (Vector3)Random.insideUnitCircle.normalized * GameManager.Instance.VisualData.CardSpawnDistance;
            newCard.transform.DOMove(randomTargetDirection, GameManager.Instance.VisualData.CardSpawnTime);

            GameManager.Instance.CurrentCards.Add(newCard);
        }

        AudioManager.Instance?.PlayClip(AudioManager.Instance.Data.CardSpawn);

    }

}