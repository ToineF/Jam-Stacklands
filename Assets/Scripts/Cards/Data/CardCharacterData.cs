using UnityEngine;

public abstract class CardCharacterData : CardData
{
    [field:Header("Character Params")]
    [field: SerializeField] public int Life { get; private set; }
    [field: SerializeField] public int DamageGiven { get; private set; }


}
