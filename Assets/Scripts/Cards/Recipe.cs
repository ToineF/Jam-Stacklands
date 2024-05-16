using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    [field: SerializeField] public bool StrictOrderOfCords { get; private set; }
    [field: SerializeField] public List<CardData> CardsNeeded { get; private set; }
    [field: SerializeField] public CardData CardToSpawn { get; private set; }
}