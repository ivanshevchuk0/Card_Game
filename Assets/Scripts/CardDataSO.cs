using UnityEngine;

public enum ElementType { None, Fire, Water, Earth }

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite cardSprite;
    public int POWER;
    public ElementType element = ElementType.None;
    [TextArea] public string description;
}