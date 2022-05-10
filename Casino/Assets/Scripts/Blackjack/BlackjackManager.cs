using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackjackManager : MonoBehaviour
{
	// All cards in a deck, these reference scripts on prefabs. To instantiate them do
	public List<Card> cards = new List<Card>();
    public Canvas canvas;
    public GameObject playerHandLocation;
    public TMP_Text playerHandValue;

    List<Card> deck = new List<Card>();
    bool playerTurn = true;

    Card dealerHidden;
    List<Card> dealerHand = new List<Card>();
    List<Card> playerHand = new List<Card>();

    void Start()
    {
        Card ace = new Card();
        ace.value = Card.eValue.ACE;
        playerHand.Add(ace);
        playerHand.Add(ace);
        playerHand.Add(ace);
        print(CalculateHandValue(playerHand));
    }

    void Update()
    {
        
    }

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

    public void DisplayPlayerCards()
	{
        playerHandValue.text = CalculateHandValue(playerHand).ToString();
        foreach (Transform c in playerHandLocation.transform) Destroy(c.gameObject);
        foreach (Card card in playerHand)
            Instantiate(card.gameObject, playerHandLocation.transform);
	}

    public Card GetCard()
    {
        Card card = deck[0];
        deck.RemoveAt(0);
        return card;
    }

    public void HitPlayer()
	{
        if (playerHand.Count == 0) return; // button won't work until cards are dealt.
        playerHand.Add(GetCard());
        DisplayPlayerCards();
	}

    public void HitDealer()
	{

	}

    public int CalculateHandValue(List<Card> cards)
	{
        int aces = 0;
        int result = 0;

        foreach(Card card in cards)
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

    public void Stay()
    {
        playerTurn = false;
    }

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
