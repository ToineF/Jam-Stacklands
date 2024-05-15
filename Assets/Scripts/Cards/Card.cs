using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [field: SerializeField] public CardData Data { get; private set; }



    public void CheckStack(DraggableCard topCard)
    {
        //Debug.Log(topCard.gameObject.name);

        List<DraggableCard> cardsToCheck = new List<DraggableCard>();
        AddToCheckStack(topCard, ref cardsToCheck);

        for (int i = 0; i < GameManager.Instance.Recipes.Length; i++)
        {
            int checkCardsCount = 0;
            var recipe = GameManager.Instance.Recipes[i];
            List<int> indexes = new List<int>();

            for (int j = 0; j < cardsToCheck.Count; j++)
            {
                if (recipe.StrictOrderOfCords)
                {
                    indexes.Add(j);
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
                            if (k == index)
                            {
                                willBreak = true;
                                break;
                            }
                        }

                        if (willBreak) continue;

                        if (recipe.CardsNeeded[k].Data == cardsToCheck[j].Card.Data)
                        {
                            indexes.Add(k);
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

    private void StartRecipe(Recipe recipe, List<DraggableCard> allCards, List<int> usedCardsIndexes)
    {
        // Delete cards if needed
        for (int i = 0; i < usedCardsIndexes.Count; i++)
        {
            var card = allCards[i];
            var cardData = card.Card.Data;
            if (cardData as CardResourceData != null)
            {
                card.ParentDraggable?.ResetCurrentDraggable(true, false);
                card.ChildDraggable?.ResetCurrentDraggable(false, true);
                Destroy(card.gameObject);
            }
        }

        // Spawn New Card
        Card newCard = Instantiate(recipe.CardToSpawn, allCards[usedCardsIndexes[0]].transform.position, Quaternion.identity, transform.parent);
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
