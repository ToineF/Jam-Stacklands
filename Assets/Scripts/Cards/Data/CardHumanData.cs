using UnityEngine;

[CreateAssetMenu(fileName = "Human", menuName = "Cards/Human")]
public class CardHumanData : CardCharacterData
{
    [field: Header("Human Params")]
    [field:SerializeField] public int ResourcesStolen { get; private set; }
}