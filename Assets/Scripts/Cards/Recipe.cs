using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    [field: SerializeField] public bool StrictOrderOfCords { get; private set; }
    [field: SerializeField] public List<Card> CardsNeeded { get; private set; }
    [field: SerializeField] public Card CardToSpawn { get; private set; }
}