using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackjackManager : MonoBehaviour
{

    List<Card> deck = new List<Card>();
    List<Card> cards = new List<Card>();
    bool playerTurn = true;

    Card dealerHidden;
    List<Card> dealerHand = new List<Card>();
    List<Card> playerHand = new List<Card>();

    void Start()
    {
        GenerateCards();
        Shuffle();
        playerHand.Add(Hit());
        dealerHidden = Hit();
        playerHand.Add(Hit());
        dealerHand.Add(Hit());
    }

    void Update()
    {
        
    }

    public void Deal()
    {

    }

    public Card Hit()
    {
        Card card = deck[0];
        deck.RemoveAt(0);
        return card;
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

    private void GenerateCards()
    {
        List<Card.eValue> values = new List<Card.eValue>() { Card.eValue.ACE, Card.eValue.TWO, Card.eValue.THREE, Card.eValue.FOUR, Card.eValue.FIVE, Card.eValue.SIX, Card.eValue.SEVEN, Card.eValue.EIGHT, Card.eValue.NINE, Card.eValue.TEN, Card.eValue.JACK, Card.eValue.QUEEN, Card.eValue.KING };
        List<Card.eSuit> suits = new List<Card.eSuit>() { Card.eSuit.HEARTS, Card.eSuit.SPADES, Card.eSuit.DIAMONDS, Card.eSuit.CLOVERS };

        for (int i = 0; i < values.Count; i++)
        {
            for (int j = 0; j < suits.Count; j++)
            {
                cards.Add(new Card(suits[j], values[i]));
            }
        }
    }
}

public class Card : MonoBehaviour
{
    public enum eValue
    {
        ACE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        TEN,
        JACK,
        QUEEN,
        KING
    }

    public enum eSuit
    {
        HEARTS,
        SPADES,
        DIAMONDS,
        CLOVERS
    }

    public eSuit suit = eSuit.SPADES;
    public eValue value = eValue.ACE;

    public Card(eSuit suit, eValue value)
    {
        this.suit = suit;
        this.value = value;
    }
}
