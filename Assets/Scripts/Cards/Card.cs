using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [field: SerializeField] public CardData Data { get; set; }
    public bool HasCommitedMurder { get; set; }

    [Header("References")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _recipeGroup;
    [SerializeField] private Image _recipeFillImage;
    [SerializeField] private Image _murderCompletedImage;
    [SerializeField] private TMP_Text _lifeText;
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private TMP_Text _sellValueText;
    [SerializeField] private Image _cardTypeImage;

    private float _currentRecipeTime;
    private float _currentRecipeCraftSpeed;
    private Coroutine _cookRoutine;

    // character
    public bool IsCharacter { get => _character != null; }
    public float CurrentLife { get => _currentLife; set => _currentLife = value; }
    public bool IsAlive { get => CurrentLife > 0; }

    private float _currentLife;
    private CardCharacterData _character;

    private void Start()
    {
        CreateData();
        UpdateData();
    }

    private void CreateData()
    {
        if (Data.Type == CardData.CardType.Human || Data.Type == CardData.CardType.Demonic)
        {
            _character = Data as CardCharacterData;
            CurrentLife = _character.Life;
        }
        else
        {
            _lifeText.gameObject.SetActive(false);
            _damageText.gameObject.SetActive(false);
        }
    }

    public void UpdateData()
    {
        _nameText.text = Data.Name;
        _image.sprite = Data.Sprite;
        if (IsCharacter) {
            _lifeText.text = CurrentLife.ToString();
            _damageText.text = _character.DamageGiven.ToString();
        }
        _sellValueText.text = Data.SellPrice.ToString();
        _cardTypeImage.sprite = GameManager.Instance?.TypeImageData.GetValue(Data.Type);
        _murderCompletedImage.gameObject.SetActive(HasCommitedMurder);
    }

    public void CheckStack(DraggableCard topCard)
    {
        //Debug.Log(topCard.gameObject.name);

        List<DraggableCard> cardsToCheck = new List<DraggableCard>();
        AddToCheckStack(topCard, ref cardsToCheck);

        for (int i = 0; i < GameManager.Instance.Recipes.Length; i++)
        {
            int checkCardsCount = 0;
            var recipe = GameManager.Instance.Recipes[i];
            Dictionary<int, bool> indexes = new Dictionary<int, bool>();
            List<int> checkedIndexes = new List<int>();

            for (int j = 0; j < cardsToCheck.Count; j++)
            {
                if (recipe.StrictOrderOfCords)
                {
                    indexes.Add(j, !recipe.CardsNeeded[j].IsDestroyedAfterCraft);
                    if (cardsToCheck[j].Card.Data != recipe.CardsNeeded[j].Data) break;
                    checkCardsCount++;
                }
                else
                {
                    for (int k = 0; k < recipe.CardsNeeded.Count; k++)
                    {
                        bool willBreak = false;
                        foreach (var index in checkedIndexes)
                        {
                            if (k == index)
                            {
                                willBreak = true;
                                break;
                            }
                        }

                        if (willBreak) continue;

                        if (recipe.CardsNeeded[k].Data == cardsToCheck[j].Card.Data)
                        {
                            indexes.Add(j, !recipe.CardsNeeded[k].IsDestroyedAfterCraft);
                            checkedIndexes.Add(k);
                            checkCardsCount++;
                            break;
                        }
                    }
                }
            }
            //Debug.Log(checkCardsCount + " " + recipe.CardsNeeded.Count);

            if (checkCardsCount >= recipe.CardsNeeded.Count)
            {
                // Recipe completed
                //Debug.Log("recipe completed");
                topCard.Card.StartRecipe(recipe, cardsToCheck, indexes);
                break;
            }
        }

    }

    private void StartRecipe(Recipe recipe, List<DraggableCard> allCards, Dictionary<int, bool> usedCardsIndexes)
    {
        _currentRecipeTime = 0;
        _currentRecipeCraftSpeed = recipe.CraftSpeed;
        _recipeGroup.alpha = 1;
        _cookRoutine = StartCoroutine(CookRecipe(recipe, allCards, usedCardsIndexes));
    }

    public void StopRecipe()
    {
        _recipeGroup.alpha = 0;
        if (_cookRoutine != null) StopCoroutine(_cookRoutine);
    }

    private IEnumerator CookRecipe(Recipe recipe, List<DraggableCard> allCards, Dictionary<int, bool> usedCardsIndexes)
    {
        yield return new WaitForSeconds(recipe.CraftSpeed);

        EndRecipe(recipe, allCards, usedCardsIndexes);
    }

    private void Update()
    {
        UpdateCraftCount();
    }

    private void UpdateCraftCount()
    {
        var timeRemaining = _currentRecipeCraftSpeed - _currentRecipeTime;
        if (_cookRoutine != null && (timeRemaining > GameManager.Instance.CurrentMaxCookTime || gameObject == GameManager.Instance.CurrentMaxCookCard))
        {
            GameManager.Instance.CurrentMaxCookTime = Mathf.Max(timeRemaining, 0);
            GameManager.Instance.CurrentMaxCookCard = gameObject;
        }
        _currentRecipeTime += Time.deltaTime;
        _recipeFillImage.fillAmount = _currentRecipeTime / _currentRecipeCraftSpeed;
    }

    private void EndRecipe(Recipe recipe, List<DraggableCard> allCards, Dictionary<int, bool> usedCardsIndexes)
    {
        _recipeGroup.alpha = 0;

        // Delete cards if needed
        int lastKey = 0;
        foreach (var index in usedCardsIndexes)
        {
            lastKey = index.Key;
            var card = allCards[index.Key];
            var cardData = card.Card.Data;
            if (!index.Value)
            {
                card.DestroyCard();
            }
        }

        // Spawn New Card
        Card newCard = Instantiate(GameManager.Instance.CardPrefab.Card, allCards[lastKey].transform.position, Quaternion.identity, transform.parent);
        newCard.Data = recipe.CardToSpawn;
        newCard.UpdateData();
        Vector3 randomTargetDirection = newCard.transform.position + (Vector3)Random.insideUnitCircle.normalized * GameManager.Instance.VisualData.CardSpawnDistance;
        newCard.transform.DOMove(randomTargetDirection, GameManager.Instance.VisualData.CardSpawnTime);
    }

    public void AddToCheckStack(DraggableCard current, ref List<DraggableCard> list)
    {
        if (current.ParentDraggable != null)
        {
            AddToCheckStack(current.ParentDraggable, ref list);
        }
        list.Add(current);

    }
}
