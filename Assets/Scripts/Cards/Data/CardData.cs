using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{

    [field: Header("General Params")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public List<CardData> CardsThatCanBeStackedOn { get; private set; }
    [field: SerializeField] public int SellPrice { get; private set; }

}