using UnityEngine;

public enum ElementType { None, Fire, Water, Earth }
public enum AbilityType { 
    None, 
    DrawExtra,      // Draw extra cards when played
    StealCard,     // Steal opponent's card on win
    DestroyOnLoss,  // Destroy opponent's card on loss
    DoublePower,    // Double power when losing
    ReviveRandom    // Revive random card from deck
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public string cardName;
    public Sprite cardSprite;
    [TextArea] public string description;
    
    [Header("Stats")]
    public int POWER;
    public ElementType element = ElementType.None;
    
    [Header("Special Ability")]
    public AbilityType ability = AbilityType.None;
    public int abilityValue = 1; // Number of cards to draw/steal/etc
    
    [Header("Visuals")]
    public Color abilityHighlightColor = Color.yellow;
    public Sprite abilityIcon;
}