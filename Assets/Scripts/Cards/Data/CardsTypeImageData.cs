using System;
using System.Collections.Generic;
using UnityEngine;
using static CardData;

[CreateAssetMenu(fileName = "CardTypeImage", menuName = "CardTypeImage")]
public class CardsTypeImageData : ScriptableObject
{
    [field: SerializeField] public List<ImageTypePair> ImagesTypesPairs { get; private set; }

    public Sprite GetValue(CardType type)
    {
        for (int i = 0; i < ImagesTypesPairs.Count; i++)
        {
            if (type != ImagesTypesPairs[i].Type) continue;

            return ImagesTypesPairs[i].Sprite;
        }
        return ImagesTypesPairs[0].Sprite;
    }
}

[Serializable]
public class ImageTypePair
{
    [field: SerializeField] public CardType Type { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }


}