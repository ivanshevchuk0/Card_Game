using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardDataSO : ScriptableObject
{
    public Sprite cardSprite;
    public int POWER;
}