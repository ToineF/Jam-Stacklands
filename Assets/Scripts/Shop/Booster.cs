using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Booster : MonoBehaviour, IPointerClickHandler
{
    public BoosterData Data
    {
        get => _data;
        set
        {
            _data = value;
        }
    }

    [SerializeField] private BoosterData _data;
    [SerializeField] private Image _image;

    private void Start()
    {
        _image.sprite = _data.Sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CardData.SpawnMultipleCards(_data, transform);

        Destroy(gameObject);
    }
}
