using UnityEngine;

public class Card : MonoBehaviour
{
    [field: SerializeField] public CardData Data { get; private set; }

    public void CheckInteraction(DraggableCard topCard)
    {
        Debug.Log(topCard.Card.Data.Name);

        CardData topCardData = topCard.Card.Data;
        CardCharacterData charact = topCardData as CardCharacterData;

    }
}
