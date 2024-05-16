using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field:Header("References")]
    [field: SerializeField] public DragNDropVisualData VisualData { get; private set; }
    [field: SerializeField] public Transform MinDragNDropZone { get; private set; }
    [field: SerializeField] public Transform MaxDragNDropZone { get; private set; }
    [field: SerializeField] public Recipe[] Recipes { get; private set; }
    [field: SerializeField] public DraggableCard CardPrefab { get; private set; }
    [field: SerializeField] public CardsTypeImageData TypeImageData { get; private set; }
    public float CurrentMaxCookTime { get; set; }
    public GameObject CurrentMaxCookCard { get; set; }

    [Header("Params")]
    [SerializeField]
    private float fastForwardTimeScale;
    
    private bool _isFastForwarding;
    public bool IsFastForwarding
    {
        get => _isFastForwarding;
        set
        {
            Time.timeScale = value ? fastForwardTimeScale : 1.0f;
            _isFastForwarding = value;
            OnFastForward.Invoke(value);
        }
    }

    private bool _isPaused;
    public bool IsPaused
    {
        get => _isPaused;
        set
        {
            _isPaused = value;
            OnPauseStateChanged.Invoke(value);
        }
    }

    public List<Card> CurrentCards { get; } = new List<Card>();
    public bool HasEnemies
    {
        get => CurrentCards.Any(card => card.Data.Type == CardData.CardType.Human);
    }

    public Action<bool> OnPauseStateChanged;
    public Action<bool> OnFastForward;

    private void Awake()
    {
        Instance = this;
    }
}
