using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoosterShop : MonoBehaviour, IDropHandler
{
    [SerializeField] private Booster _boosterToSpawn;
    [SerializeField] private TMP_Text _priceText;

    private int _currentPrice;

    private void Awake()
    {
        ResetPrice();
    }

    private void ResetPrice()
    {
        _currentPrice = _boosterToSpawn.Data.Price;
        _priceText.text = _currentPrice.ToString();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        DraggableCard newDraggable = droppedObject.GetComponent<DraggableCard>();

        if (newDraggable == null) return;

        UpdateShop(newDraggable);

    }

    private void UpdateShop(DraggableCard card)
    {
        _currentPrice -= card.Card.Data.SellPrice;
        _priceText.text = _currentPrice.ToString();

        if (_currentPrice <= 0)
        {
            Instantiate(_boosterToSpawn, transform.position, Quaternion.identity);
            ResetPrice();
        }
    }
}
