using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackjackManager : MonoBehaviour
{
	/// <summary>
    /// Prefabs for each card in a deck.
    /// </summary>
	public List<Card> cards = new List<Card>();
    //public Canvas canvas; // unused
    /// <summary>
    /// The parent gameobject where player cards should be instantiated.
    /// </summary>
    public GameObject playerHandLocation;
    /// <summary>
    /// The text display for the player's hand value.
    /// </summary>
    public TMP_Text playerHandValue;

    /// <summary>
    /// A deck of prefab cards which have not been dealt.
    /// </summary>
    private List<Card> deck = new List<Card>();
    bool playerTurn = true;

    Card dealerHidden;
    /// <summary>
    /// The cards in the dealer's hand
    /// </summary>
    List<Card> dealerHand = new List<Card>();
    /// <summary>
    /// The cards in the player's hand
    /// </summary>
    List<Card> playerHand = new List<Card>();

    public void Deal()
    {
        Shuffle();
        playerHand.Clear();
        dealerHand.Clear();

        foreach(Transform card in playerHandLocation.transform)
		{
            Destroy(card.gameObject);
		}

        for(int i = 0; i < 2; i++)
		{
            playerHand.Add(GetCard());
            dealerHand.Add(GetCard());
		}

        DisplayPlayerCards();
    }

    /// <summary>
    /// Updates the player card display and value display
    /// </summary>
    public void DisplayPlayerCards()
	{
        playerHandValue.text = CalculateHandValue(playerHand).ToString();
        foreach (Transform c in playerHandLocation.transform) Destroy(c.gameObject);
        foreach (Card card in playerHand)
            Instantiate(card.gameObject, playerHandLocation.transform);
	}

    /// <summary>
    /// Pops off the first card in the deck and returns it.
    /// </summary>
    public Card GetCard()
    {
        Card card = deck[0];
        deck.RemoveAt(0);
        return card;
    }

    /// <summary>
    /// Method called when player clicks "Hit" button.
    /// </summary>
    public void HitPlayer()
	{
        // button won't work if cards are not dealt or if player's hand is over 21
        if (playerHand.Count == 0 || CalculateHandValue(playerHand) > 21) return; 
        playerHand.Add(GetCard());
        DisplayPlayerCards();
	}

    /// <summary>
    /// Calculates and returns the value of the hand given. If ace as 11 puts the value over 21 then it is changed to 1.
    /// </summary>
    public int CalculateHandValue(List<Card> hand)
	{
        int aces = 0;
        int result = 0;

        foreach(Card card in hand)
		{
			switch (card.value)
			{
				case Card.eValue.ACE:
                    aces++;
                    result += 11;
                    break;
				case Card.eValue.JACK:
				case Card.eValue.QUEEN:
				case Card.eValue.KING:
                    result += 10;
					break;
				default:
                    result += (int)card.value + 1;
                    break;
			}
		}
        for (;aces > 0 && result > 21; aces--)
		{
            result -= 10;
		}

        return result;
	}

    /// <summary>
    /// Resets the deck to a fresh full randomized deck.
    /// </summary>
    public void Shuffle()
    {
        deck.Clear();

        List<Card> tempCards = new List<Card>(cards);

        for (int i = 0; i < cards.Count; i++)
        {
            Card card = tempCards[Random.Range(0, tempCards.Count)];
            deck.Add(card);
            tempCards.Remove(card);
        }
    }

    //private void GenerateCards()
    //{
    //    List<Card.eValue> values = new List<Card.eValue>() { Card.eValue.ACE, Card.eValue.TWO, Card.eValue.THREE, Card.eValue.FOUR, Card.eValue.FIVE, Card.eValue.SIX, Card.eValue.SEVEN, Card.eValue.EIGHT, Card.eValue.NINE, Card.eValue.TEN, Card.eValue.JACK, Card.eValue.QUEEN, Card.eValue.KING };
    //    List<Card.eSuit> suits = new List<Card.eSuit>() { Card.eSuit.HEARTS, Card.eSuit.SPADES, Card.eSuit.DIAMONDS, Card.eSuit.CLOVERS };

    //    for (int i = 0; i < values.Count; i++)
    //    {
    //        for (int j = 0; j < suits.Count; j++)
    //        {
    //            cards.Add(new Card(suits[j], values[i]));
    //        }
    //    }
    //}
}
