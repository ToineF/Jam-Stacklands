using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [field: SerializeField] public CardData Data { get; set; }

    [Header("References")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _image;

    private void Awake()
    {
        UpdateData();
    }

    public void UpdateData()
    {
        _nameText.text = Data.Name;
        _image.sprite = Data.Sprite;
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

            for (int j = 0; j < cardsToCheck.Count; j++)
            {
                if (recipe.StrictOrderOfCords)
                {
                    indexes.Add(j, recipe.CardsNeeded[j].IsDestroyedAfterCraft);
                    if (cardsToCheck[j].Card.Data != recipe.CardsNeeded[j].Data) break;
                    checkCardsCount++;
                }
                else
                {
                    for (int k = 0; k < recipe.CardsNeeded.Count; k++)
                    {
                        bool willBreak = false;
                        foreach (var index in indexes)
                        {
                            if (k == index.Key)
                            {
                                willBreak = true;
                                break;
                            }
                        }

                        if (willBreak) continue;

                        if (recipe.CardsNeeded[k].Data == cardsToCheck[j].Card.Data)
                        {
                            indexes.Add(k, recipe.CardsNeeded[k].IsDestroyedAfterCraft);
                            checkCardsCount++;
                            break;
                        }
                    }
                }
            }
            Debug.Log(checkCardsCount + " " + recipe.CardsNeeded.Count);

            if (checkCardsCount >= recipe.CardsNeeded.Count)
            {
                // Recipe completed
                Debug.Log("recipe completed");
                StartRecipe(recipe, cardsToCheck, indexes);
                break;
            }
        }

    }

    private void StartRecipe(Recipe recipe, List<DraggableCard> allCards, Dictionary<int, bool> usedCardsIndexes)
    {
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
        Card newCard = Instantiate(GameManager.Instance.CardPrefab, allCards[lastKey].transform.position, Quaternion.identity, transform.parent);
        newCard.Data = recipe.CardToSpawn;
        newCard.UpdateData();
        Vector3 randomTargetDirection = newCard.transform.position + (Vector3)Random.insideUnitCircle.normalized * GameManager.Instance.VisualData.CardSpawnDistance;
        newCard.transform.DOMove(randomTargetDirection, GameManager.Instance.VisualData.CardSpawnTime);

    }

    public void AddToCheckStack(DraggableCard current, ref List<DraggableCard> list)
    {
        list.Add(current);
        if (current.ParentDraggable == null) return;

        AddToCheckStack(current.ParentDraggable, ref list);
    }
}
