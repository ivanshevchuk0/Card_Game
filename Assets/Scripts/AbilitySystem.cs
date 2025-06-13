using UnityEngine;

public static class AbilitySystem
{
    public static void TriggerAbility(
        AbilityType ability,
        int value,
        CardGameController game,
        CardDataSO thisCard,
        CardDataSO otherCard,
        bool wonRound)
    {
        switch (ability)
        {
            case AbilityType.DrawExtra:
                if (wonRound)
                {
                    game.DrawCards(value); // value = number to draw
                    game.ShowAbilityMessage($"{thisCard.cardName}: Drew {value} extra cards!");
                }
                break;
                
            case AbilityType.StealCard:
                if (wonRound && otherCard != null)
                {
                    game.StealCard(otherCard);
                    game.ShowAbilityMessage($"{thisCard.cardName}: Stole opponent's {otherCard.cardName}!");
                }
                break;
                
            case AbilityType.DestroyOnLoss:
                if (!wonRound && otherCard != null)
                {
                    game.DestroyCard(otherCard);
                    game.ShowAbilityMessage($"{thisCard.cardName}: Destroyed {otherCard.cardName} on loss!");
                }
                break;
                
            case AbilityType.DoublePower:
                if (!wonRound)
                {
                    game.BoostCardPower(thisCard, 2f);
                    game.ShowAbilityMessage($"{thisCard.cardName}: Power doubled when losing!");
                }
                break;
                
            case AbilityType.ReviveRandom:
                if (wonRound)
                {
                    game.ReviveRandomCard();
                    game.ShowAbilityMessage($"{thisCard.cardName}: Revived a random card!");
                }
                break;
        }
    }
}