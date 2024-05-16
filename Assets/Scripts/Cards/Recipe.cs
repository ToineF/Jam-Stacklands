using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    [field: SerializeField] public bool StrictOrderOfCords { get; private set; }
    [field: SerializeField] public List<CardIngredient> CardsNeeded { get; private set; }
    [field: SerializeField] public CardData CardToSpawn { get; private set; }
    [field: SerializeField] public float CraftSpeed { get; private set; }
}

[Serializable]
public class CardIngredient
{
    [field:SerializeField] public CardData Data { get; private set; }
    [field:SerializeField] public bool IsDestroyedAfterCraft { get; private set;}
}