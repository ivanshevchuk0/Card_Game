using UnityEngine;

public class CardData : MonoBehaviour
{
    private CardDataSO cardData;

    public void SetCard(CardDataSO data)
    {
        cardData = data;
    }

    public CardDataSO GetCard()
    {
        return cardData;
    }
}