using UnityEngine;

[CreateAssetMenu(fileName = "Demon", menuName = "Cards/Demon")]
public class CardDemonData : CardCharacterData
{
    [field: Header("Demon Params")]
    [field: SerializeField] public float CraftSpeed { get; private set; }
}