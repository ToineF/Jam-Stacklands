using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoosterShop : Shop, IDropHandler
{
    [SerializeField] private Booster _boosterPrefab;
    [SerializeField] private BoosterData _boosterToSpawn;
    [SerializeField] private TMP_Text _priceText;

    private int _currentPrice;

    private void Awake()
    {
        ResetPrice();
    }

    private void ResetPrice()
    {
        _currentPrice = _boosterToSpawn.Price;
        _priceText.text = _currentPrice.ToString();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        DraggableCard newDraggable = droppedObject.GetComponent<DraggableCard>();

        if (newDraggable.Card.Data as CardOfferingData == null) return;
        //if (newDraggable == null) return;

        UpdateShop(newDraggable);

    }

    private void UpdateShop(DraggableCard card)
    {
        card.DoForAllChildrenBefore(() =>
        {
            _currentPrice -= card.Card.Data.SellPrice;
            _priceText.text = _currentPrice.ToString();
        });
        card.DestroyAllChildren();


        if (_currentPrice <= 0)
        {
            Booster booster = Instantiate(_boosterPrefab, transform.position, Quaternion.identity, transform.parent.parent);
            Vector3 targetPosition = transform.position + Vector3.down * GameManager.Instance.VisualData.BoosterEjectionSpeed;
            booster.transform.DOMove(targetPosition, GameManager.Instance.VisualData.BoosterEjectionTime);
            booster.Data = _boosterToSpawn;
            ResetPrice();
        }
    }
}
