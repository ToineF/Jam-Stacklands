using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardDataSpawnable : ScriptableObject
{
    [field: SerializeField] public int NumberToSpawn { get; private set; }
    [field: SerializeField] public List<CardProbabilityPair> SpawnableCards { get; private set; }
}
