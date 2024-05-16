using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class OfferingShop : Shop, IDropHandler
{
    [SerializeField] private CardData _offeringData;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        DraggableCard newDraggable = droppedObject.GetComponent<DraggableCard>();

        //if (newDraggable.Card.Data as CardOfferingData == null) return;
        if (newDraggable == null) return;

        UpdateShop(newDraggable);

    }

    private void UpdateShop(DraggableCard card)
    {
        int offeringsToSpawn = 0;
        card.DoForAllChildrenBefore(() =>
        {
            offeringsToSpawn += card.Card.Data.SellPrice;
        });

        card.DestroyAllChildren();

        DraggableCard previousCard = card;
        for (int i = 0; i < offeringsToSpawn; i++)
        {
            DraggableCard newCard = Instantiate(GameManager.Instance.CardPrefab, transform.position, Quaternion.identity, transform.parent.parent);
            Vector3 targetPosition = transform.position + Vector3.down * GameManager.Instance.VisualData.BoosterEjectionSpeed;
            newCard.transform.DOMove(targetPosition, GameManager.Instance.VisualData.BoosterEjectionTime);
            newCard.Card.Data = _offeringData;
            newCard.Card.UpdateData();
            if (i > 0)
            {
                newCard.SetParentDraggable(previousCard);
                previousCard.SetChildrenDraggable(newCard);
            }
            previousCard = newCard;
        }
    }
}
