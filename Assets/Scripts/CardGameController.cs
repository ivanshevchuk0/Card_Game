using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CardGameController : MonoBehaviour
{
    [SerializeField] private List<CardDataSO> deck;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform computerSpawn;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private int startingCards = 5;
    
    [SerializeField] private Transform playerCenterPos;
    [SerializeField] private Transform computerCenterPos;
    
    private List<CardDataSO> playerHand = new();
    private List<CardDataSO> computerHand = new();

    private void Start()
    {
        // Shuffle deck
        deck = deck.OrderBy(card => Random.value).ToList();
        
        // Deal starting cards
        for (int i = 0; i < startingCards && deck.Count >= 2; i++)
        {
            playerHand.Add(deck[0]);
            deck.RemoveAt(0);
            computerHand.Add(deck[0]);
            deck.RemoveAt(0);
        }
        
        DisplayHands();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayRound();
        }
    }

    void DisplayHands()
    {
        ClearPreviousCards();

        Vector3 playerCenter = playerSpawn.position;
        Vector3 computerCenter = computerSpawn.position;

        Quaternion yRotation = Quaternion.Euler(0, 0, 0);

        // Display player hand (face down)
        for (int i = 0; i < playerHand.Count; i++)
        {
            Vector3 offset = new Vector3(i * 1.1f, 0, 0);
            var card = Instantiate(cardPrefab, playerCenter + offset, yRotation);
            card.GetComponent<SpriteRenderer>().sprite = backSprite;
            card.tag = "Card";
        }

        // Display computer hand (face down)
        for (int i = 0; i < computerHand.Count; i++)
        {
            Vector3 offset = new Vector3(i * 1.1f, 0, 0);
            var card = Instantiate(cardPrefab, computerCenter + offset, yRotation);
            card.GetComponent<SpriteRenderer>().sprite = backSprite;
            card.tag = "Card";
        }
    }

    void PlayRound()
    {
        if (playerHand.Count == 0 || computerHand.Count == 0)
        {
            Debug.Log(playerHand.Count == 0 ? "Computer wins!" : "Player wins!");
            return;
        }

        int pIndex = Random.Range(0, playerHand.Count);
        int cIndex = Random.Range(0, computerHand.Count);

        var playerCard = playerHand[pIndex];
        var compCard = computerHand[cIndex];

        playerHand.RemoveAt(pIndex);
        computerHand.RemoveAt(cIndex);

        DisplayCardInCenter(playerCard, compCard);

        if (playerCard.POWER > compCard.POWER)
        {
            Debug.Log("Player won the round");
            playerHand.Add(playerCard);
            playerHand.Add(compCard);
        }
        else if (playerCard.POWER < compCard.POWER)
        {
            Debug.Log("Computer won the round");
            computerHand.Add(playerCard);
            computerHand.Add(compCard);
        }
        else
        {
            Debug.Log("Draw - cards are lost");
        }
        
        Invoke(nameof(DisplayHands), 2.5f);
    }

    void DisplayCardInCenter(CardDataSO pCard, CardDataSO cCard)
    {
        GameObject playerCardCenter = Instantiate(cardPrefab, playerCenterPos.position, Quaternion.identity);
        playerCardCenter.GetComponent<SpriteRenderer>().sprite = pCard.cardSprite;
        playerCardCenter.GetComponent<CardData>().SetCard(pCard);
        playerCardCenter.tag = "Card";
        playerCardCenter.transform.rotation = Quaternion.identity;

        GameObject compCardCenter = Instantiate(cardPrefab, computerCenterPos.position, Quaternion.identity);
        compCardCenter.GetComponent<SpriteRenderer>().sprite = cCard.cardSprite;
        compCardCenter.GetComponent<CardData>().SetCard(cCard);
        compCardCenter.tag = "Card";
        compCardCenter.transform.rotation = Quaternion.identity;
    }

    void ClearPreviousCards()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Card"))
        {
            Destroy(go);
        }
    }
}