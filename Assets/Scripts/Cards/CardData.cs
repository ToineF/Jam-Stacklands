using UnityEngine;
using UnityEngine.UI;

public abstract class CardData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public int OfferingAmount { get; private set; }
}