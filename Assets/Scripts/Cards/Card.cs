using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [field: SerializeField] public CardData Data { get; private set; }



    public void CheckStack(DraggableCard topCard)
    {
        Debug.Log(topCard.gameObject.name);

        CardData topCardData = topCard.Card.Data;

        List<DraggableCard> cardsToCheck = new List<DraggableCard>();
        AddToCheckStack(topCard, ref cardsToCheck);

        Debug.LogWarning(cardsToCheck.Count);
        //foreach (var recipe in GameManager.Instance.Recipes)
        //{
        //    cardsToCheck = 
        //    if (recipe.StrictOrderOfCords)
        //}
        
    }

    public void AddToCheckStack(DraggableCard current, ref List<DraggableCard> list)
    {
        list.Add(current);
        if (current.ParentDraggable == null) return;

        AddToCheckStack(current.ParentDraggable, ref list);
    }
}
