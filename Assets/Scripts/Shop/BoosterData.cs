using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Booster", menuName = "Booster")]
public class BoosterData : ScriptableObject
{
    [field: SerializeField] public int NumberToSpawn { get; private set; }
    [field: SerializeField] public List<CardProbabilityPair> SpawnableCards { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
}

[Serializable]
public class CardProbabilityPair
{
    [field: SerializeField] public CardData Card { get; private set; }
    [field: SerializeField] public int Probability { get; private set; }

}