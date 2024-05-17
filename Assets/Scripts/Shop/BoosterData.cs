using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Booster", menuName = "Booster")]
public class BoosterData : CardDataSpawnable
{
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public Sprite Sprite{ get; private set; }
}

[Serializable]
public class CardProbabilityPair
{
    [field: SerializeField] public CardData Card { get; private set; }
    [field: SerializeField] public int Probability { get; private set; }

}