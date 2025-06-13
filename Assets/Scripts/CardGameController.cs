using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CardGameController : MonoBehaviour
{
    [Header("Game Setup")]
    [SerializeField] private List<CardDataSO> deck;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform computerSpawn;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private int startingCards = 5;
    
    [Header("Battle Positions")]
    [SerializeField] private Transform playerCenterPos;
    [SerializeField] private Transform computerCenterPos;
    
    [Header("Elemental Mechanics")]
    [SerializeField] private GameObject elementEffectPrefab;
    [SerializeField] private Sprite fireIcon;
    [SerializeField] private Sprite waterIcon;
    [SerializeField] private Sprite earthIcon;
    [SerializeField] private Color fireColor = Color.red;
    [SerializeField] private Color waterColor = Color.blue;
    [SerializeField] private Color earthColor = Color.green;
    
    [Header("UI Elements")]
    [SerializeField] private TMPro.TextMeshProUGUI playerCardCountText;
    [SerializeField] private TMPro.TextMeshProUGUI computerCardCountText;
    [SerializeField] private TMPro.TextMeshProUGUI resultText;
    
    private List<CardDataSO> playerHand = new();
    private List<CardDataSO> computerHand = new();
    private bool roundInProgress = false;

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        // Clear hands and shuffle
        playerHand.Clear();
        computerHand.Clear();
        deck = deck.OrderBy(card => Random.value).ToList();
        
        // Deal starting cards
        for (int i = 0; i < startingCards && deck.Count >= 2; i++)
        {
            playerHand.Add(deck[0]);
            deck.RemoveAt(0);
            computerHand.Add(deck[0]);
            deck.RemoveAt(0);
        }
        
        UpdateUI();
        DisplayHands();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !roundInProgress)
        {
            PlayRound();
        }
    }

    public void PlayRound()
    {
        if (roundInProgress) return;
        
        if (playerHand.Count == 0 || computerHand.Count == 0)
        {
            GameOver(playerHand.Count == 0 ? "Computer" : "Player");
            return;
        }

        roundInProgress = true;
        ClearPreviousCards();
        
        int pIndex = Random.Range(0, playerHand.Count);
        int cIndex = Random.Range(0, computerHand.Count);

        var playerCard = playerHand[pIndex];
        var compCard = computerHand[cIndex];

        playerHand.RemoveAt(pIndex);
        computerHand.RemoveAt(cIndex);

        DisplayCardInCenter(playerCard, compCard);
        ApplyElementalEffects(playerCard, compCard);
        
        // Calculate elemental advantages
        int playerPower = CalculateElementalPower(playerCard, compCard);
        int compPower = CalculateElementalPower(compCard, playerCard);
        
        string resultMessage;
        if (playerPower > compPower)
        {
            resultMessage = $"Player wins! {playerCard.cardName} ({playerPower}) beats {compCard.cardName} ({compPower})";
            playerHand.Add(playerCard);
            playerHand.Add(compCard);
        }
        else if (playerPower < compPower)
        {
            resultMessage = $"Computer wins! {compCard.cardName} ({compPower}) beats {playerCard.cardName} ({playerPower})";
            computerHand.Add(playerCard);
            computerHand.Add(compCard);
        }
        else
        {
            resultMessage = $"Draw! Both cards have power {playerPower}";
        }
        
        resultText.text = resultMessage;
        UpdateUI();
        
        Invoke(nameof(FinishRound), 2.5f);
    }

    private void FinishRound()
    {
        roundInProgress = false;
        DisplayHands();
    }

    private void GameOver(string winner)
    {
        resultText.text = $"{winner} wins the game!";
        Invoke(nameof(InitializeGame), 3f);
    }

    private int CalculateElementalPower(CardDataSO card, CardDataSO opponentCard)
    {
        int basePower = card.POWER;
        
        switch (card.element)
        {
            case ElementType.Fire:
                if (opponentCard.element == ElementType.Earth) 
                    return basePower * 2;
                if (opponentCard.element == ElementType.Water) 
                    return basePower / 2;
                break;
                
            case ElementType.Water:
                if (opponentCard.element == ElementType.Fire) 
                    return basePower * 2;
                if (opponentCard.element == ElementType.Earth) 
                    return basePower / 2;
                break;
                
            case ElementType.Earth:
                if (opponentCard.element == ElementType.Water) 
                    return basePower * 2;
                if (opponentCard.element == ElementType.Fire) 
                    return basePower / 2;
                break;
        }
        
        return basePower;
    }

    private void ApplyElementalEffects(CardDataSO playerCard, CardDataSO compCard)
    {
        // Player card effect
        if (playerCard.element != ElementType.None)
        {
            CreateElementEffect(playerCenterPos.position, playerCard.element);
        }
        
        // Computer card effect
        if (compCard.element != ElementType.None)
        {
            CreateElementEffect(computerCenterPos.position, compCard.element);
        }
    }

    private void CreateElementEffect(Vector3 position, ElementType element)
    {
        GameObject effect = Instantiate(elementEffectPrefab, position, Quaternion.identity);
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = GetElementColor(element);
        Destroy(effect, 2f);
    }

    void DisplayHands()
    {
        ClearPreviousCards();

        Vector3 playerCenter = playerSpawn.position;
        Vector3 computerCenter = computerSpawn.position;

        Quaternion rotation = Quaternion.Euler(0, 0, 0);

        // Display player hand (face down)
        for (int i = 0; i < playerHand.Count; i++)
        {
            Vector3 offset = new Vector3(i * 1.1f, 0, 0);
            var card = Instantiate(cardPrefab, playerCenter + offset, rotation);
            card.GetComponent<SpriteRenderer>().sprite = backSprite;
            card.tag = "Card";
        }

        // Display computer hand (face down)
        for (int i = 0; i < computerHand.Count; i++)
        {
            Vector3 offset = new Vector3(i * 1.1f, 0, 0);
            var card = Instantiate(cardPrefab, computerCenter + offset, rotation);
            card.GetComponent<SpriteRenderer>().sprite = backSprite;
            card.tag = "Card";
        }
    }

    void DisplayCardInCenter(CardDataSO pCard, CardDataSO cCard)
    {
        // Player card
        GameObject playerCardCenter = Instantiate(cardPrefab, playerCenterPos.position, Quaternion.identity);
        playerCardCenter.GetComponent<SpriteRenderer>().sprite = pCard.cardSprite;
        playerCardCenter.GetComponent<CardData>().SetCard(pCard);
        playerCardCenter.tag = "Card";
        AddElementIndicator(playerCardCenter, pCard.element);

        // Computer card
        GameObject compCardCenter = Instantiate(cardPrefab, computerCenterPos.position, Quaternion.identity);
        compCardCenter.GetComponent<SpriteRenderer>().sprite = cCard.cardSprite;
        compCardCenter.GetComponent<CardData>().SetCard(cCard);
        compCardCenter.tag = "Card";
        AddElementIndicator(compCardCenter, cCard.element);
    }

    private void AddElementIndicator(GameObject card, ElementType element)
    {
        if (element == ElementType.None) return;
        
        GameObject indicator = new GameObject("ElementIndicator");
        indicator.transform.SetParent(card.transform);
        indicator.transform.localPosition = new Vector3(0, -0.5f, -0.1f);
        SpriteRenderer sr = indicator.AddComponent<SpriteRenderer>();
        sr.sprite = GetElementSprite(element);
        sr.color = GetElementColor(element);
        sr.sortingOrder = 1;
    }

    private Sprite GetElementSprite(ElementType element)
    {
        return element switch {
            ElementType.Fire => fireIcon,
            ElementType.Water => waterIcon,
            ElementType.Earth => earthIcon,
            _ => null
        };
    }

    private Color GetElementColor(ElementType element)
    {
        return element switch {
            ElementType.Fire => fireColor,
            ElementType.Water => waterColor,
            ElementType.Earth => earthColor,
            _ => Color.white
        };
    }

    void ClearPreviousCards()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Card"))
        {
            Destroy(go);
        }
    }

    private void UpdateUI()
    {
        playerCardCountText.text = $"Player: {playerHand.Count} cards";
        computerCardCountText.text = $"Computer: {computerHand.Count} cards";
    }
}