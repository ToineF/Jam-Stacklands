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
    [field: SerializeField] public HumanHandler HumanHandler { get; private set; }
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

    public List<DraggableCard> CurrentCards { get; } = new List<DraggableCard>();
    public bool HasHumans
    {
        get => CurrentCards.Any(card => card.Card.Data.Type == CardData.CardType.Human && card.Card.Data.DisappearAfterNight);
    }
    public bool HasDemons
    {
        get => CurrentCards.Any(card => card.Card.Data.Type == CardData.CardType.Demonic);
    }


    public Action<bool> OnPauseStateChanged;
    public Action<bool> OnFastForward;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var cards = FindObjectsOfType<DraggableCard>();
        foreach (var card in cards)
        {
            CurrentCards.Add(card);
        }
    }
}
