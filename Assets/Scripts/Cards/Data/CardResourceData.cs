using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "Cards/Resource")]
public class CardResourceData : CardCharacterData
{
    [field: Header("Resource Params")]
    [field: SerializeField] public CardData CardToSpawnOnCraft { get; private set; }
    [field: SerializeField] public bool IsDestroyedAfterCraft { get; private set; } = true;
}