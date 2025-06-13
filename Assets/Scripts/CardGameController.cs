using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    
    [Header("Ability System")]
    [SerializeField] private GameObject abilityMessagePrefab;
    [SerializeField] private float abilityMessageDuration = 2f;
    
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
        playerHand.Clear();
        computerHand.Clear();
        deck = deck.OrderBy(card => Random.value).ToList();
        
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
        
        int playerPower = CalculateElementalPower(playerCard, compCard);
        int compPower = CalculateElementalPower(compCard, playerCard);
        
        string resultMessage;
        if (playerPower > compPower)
        {
            resultMessage = $"Player wins! {playerCard.cardName} ({playerPower}) beats {compCard.cardName} ({compPower})";
            playerHand.Add(playerCard);
            playerHand.Add(compCard);
            
            // Trigger abilities
            AbilitySystem.TriggerAbility(playerCard.ability, playerCard.abilityValue, this, playerCard, compCard, true);
            AbilitySystem.TriggerAbility(compCard.ability, compCard.abilityValue, this, compCard, playerCard, false);
        }
        else if (playerPower < compPower)
        {
            resultMessage = $"Computer wins! {compCard.cardName} ({compPower}) beats {playerCard.cardName} ({playerPower})";
            computerHand.Add(playerCard);
            computerHand.Add(compCard);
            
            // Trigger abilities
            AbilitySystem.TriggerAbility(compCard.ability, compCard.abilityValue, this, compCard, playerCard, true);
            AbilitySystem.TriggerAbility(playerCard.ability, playerCard.abilityValue, this, playerCard, compCard, false);
        }
        else
        {
            resultMessage = $"Draw! Both cards have power {playerPower}";
            deck.Add(playerCard);
            deck.Add(compCard);
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
                if (opponentCard.element == ElementType.Earth) return basePower * 2;
                if (opponentCard.element == ElementType.Water) return basePower / 2;
                break;
            case ElementType.Water:
                if (opponentCard.element == ElementType.Fire) return basePower * 2;
                if (opponentCard.element == ElementType.Earth) return basePower / 2;
                break;
            case ElementType.Earth:
                if (opponentCard.element == ElementType.Water) return basePower * 2;
                if (opponentCard.element == ElementType.Fire) return basePower / 2;
                break;
        }
        
        return basePower;
    }

    private void ApplyElementalEffects(CardDataSO playerCard, CardDataSO compCard)
    {
        if (playerCard.element != ElementType.None)
            CreateElementEffect(playerCenterPos.position, playerCard.element);
        if (compCard.element != ElementType.None)
            CreateElementEffect(computerCenterPos.position, compCard.element);
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

        for (int i = 0; i < playerHand.Count; i++)
        {
            Vector3 offset = new Vector3(i * 1.1f, 0, 0);
            var card = Instantiate(cardPrefab, playerCenter + offset, rotation);
            card.GetComponent<SpriteRenderer>().sprite = backSprite;
            card.tag = "Card";
        }

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
        GameObject playerCardCenter = CreateCard(pCard, playerCenterPos.position);
        // Computer card
        GameObject compCardCenter = CreateCard(cCard, computerCenterPos.position);
    }

    private GameObject CreateCard(CardDataSO cardData, Vector3 position)
    {
        GameObject card = Instantiate(cardPrefab, position, Quaternion.identity);
        card.GetComponent<SpriteRenderer>().sprite = cardData.cardSprite;
        card.GetComponent<CardData>().SetCard(cardData);
        card.tag = "Card";
        
        // Add element indicator
        if (cardData.element != ElementType.None)
        {
            AddElementIndicator(card, cardData.element);
        }
        
        // Add ability indicator
        if (cardData.ability != AbilityType.None)
        {
            AddAbilityIndicator(card, cardData);
        }
        
        return card;
    }

    private void AddElementIndicator(GameObject card, ElementType element)
    {
        GameObject indicator = new GameObject("ElementIndicator");
        indicator.transform.SetParent(card.transform);
        indicator.transform.localPosition = new Vector3(0, -0.5f, -0.1f);
        SpriteRenderer sr = indicator.AddComponent<SpriteRenderer>();
        sr.sprite = GetElementSprite(element);
        sr.color = GetElementColor(element);
        sr.sortingOrder = 1;
    }

    private void AddAbilityIndicator(GameObject card, CardDataSO cardData)
    {
        GameObject indicator = new GameObject("AbilityIndicator");
        indicator.transform.SetParent(card.transform);
        indicator.transform.localPosition = new Vector3(0, 0.5f, -0.1f);
        
        SpriteRenderer sr = indicator.AddComponent<SpriteRenderer>();
        sr.sprite = cardData.abilityIcon;
        sr.color = cardData.abilityHighlightColor;
        sr.sortingOrder = 2;
        
        indicator.AddComponent<AbilityPulse>();
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

    // Ability system methods
    public void DrawCards(int count)
    {
        for (int i = 0; i < count && deck.Count > 0; i++)
        {
            playerHand.Add(deck[0]);
            deck.RemoveAt(0);
        }
        UpdateUI();
    }

    public void StealCard(CardDataSO card)
    {
        playerHand.Add(card);
        computerHand.Remove(card);
        UpdateUI();
    }

    public void DestroyCard(CardDataSO card)
    {
        // Could implement discard pile logic here
        UpdateUI();
    }

    public void BoostCardPower(CardDataSO card, float multiplier)
    {
        card.POWER = Mathf.RoundToInt(card.POWER * multiplier);
    }

    public void ReviveRandomCard()
    {
        if (deck.Count > 0)
        {
            playerHand.Add(deck[0]);
            deck.RemoveAt(0);
            UpdateUI();
        }
    }

    public void ShowAbilityMessage(string message)
    {
        GameObject msg = Instantiate(abilityMessagePrefab, transform);
        msg.GetComponent<TMPro.TextMeshProUGUI>().text = message;
        Destroy(msg, abilityMessageDuration);
    }

    private void UpdateUI()
    {
        playerCardCountText.text = $"Player: {playerHand.Count} cards";
        computerCardCountText.text = $"Computer: {computerHand.Count} cards";
    }
}