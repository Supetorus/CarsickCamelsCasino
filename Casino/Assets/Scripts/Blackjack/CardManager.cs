using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	public enum CardRules
	{
		Blackjack,
		Poker
	}

	public enum PokerHandValue
    {
		HighCard,
		Pair,
		TwoPair,
		ThreeOfAKind,
		Straight,
		Flush,
		FullHouse,
		FourOfAKind,
		StraightFlush,
		RoyalFlush
    }

	/// <summary>
	/// A deck of prefab cards which have not been dealt.
	/// </summary>
	private List<Card> deck = new List<Card>();
	/// <summary>
	/// Prefabs for each card in a deck.
	/// </summary>
	public List<Card> cards = new List<Card>();

	/// <summary>
	/// Deals count cards to each hand in hands.
	/// </summary>
	/// <param name="hands">An array of lists of cards</param>
	/// <param name="count">The number of cards that should be dealt to each hand.</param>
	public void Deal(int count, bool clear, params List<Card>[] hands)
	{
		if (deck.Count < hands.Length * count)
			throw new System.Exception("There are not enough cards in the deck to deal, it needs to be shuffled");

		foreach (var hand in hands)
		{
			if (clear) hand.Clear();
			for (int i = 0; i < count; i++)
			{
				hand.Add(GetCard());
			}
		}
	}

	/// <summary>
	/// Pops off the first card in the deck and returns it.
	/// </summary>
	public Card GetCard()
	{
		if (cards.Count == 0) throw new System.Exception("The deck is empty, could not get a card.");

		Card card = deck[0];
		deck.RemoveAt(0);
		return card;
	}

	/// <summary>
	/// Calculates the value of the hand according to the game rules given.
	/// </summary>
	public int CalculateHandValue(List<Card> hand, CardRules rules)
	{
		switch (rules)
		{
			case CardRules.Blackjack:
				return CalculateBlackJackHand(hand);
			case CardRules.Poker:
				return CalculatePokerHand(hand);
			default:
				return 0;
		}
	}

	private int CalculateBlackJackHand(List<Card> hand)
	{
		int aces = 0;
		int result = 0;

		foreach (Card card in hand)
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
		for (; aces > 0 && result > 21; aces--)
		{
			result -= 10;
		}

		return result;
	}

	private int CalculatePokerHand(List<Card> hand)
    {
		PokerHandValue handValue;



		// if royal flush
		if (hand.Exists(c => c.value == Card.eValue.ACE) && hand.Exists(c => c.value == Card.eValue.KING) && hand.Exists(c => c.value == Card.eValue.QUEEN) && hand.Exists(c => c.value == Card.eValue.JACK) && hand.Exists(c => c.value == Card.eValue.TEN) && hand.FindAll(c => c.suit == hand[0].suit).Count == 5)
			handValue = PokerHandValue.RoyalFlush;
		// if straight flush
		else if (false)
			handValue = PokerHandValue.StraightFlush;
		// if four of kind
		else if (hand.FindAll(c => c.value == hand[0].value).Count == 4 || hand.FindAll(c => c.value == hand[1].value).Count == 4)
			handValue = PokerHandValue.FourOfAKind;
		// if full house
		else if (false)
			handValue = PokerHandValue.FullHouse;
		// if flush
		else if (hand.FindAll(c => c.suit == hand[0].suit).Count == 5) 
			handValue = PokerHandValue.Flush;
		// if straight
		else if (false)
			handValue = PokerHandValue.Straight;
		// if three of kind
		else if (hand.FindAll(c => c.value == hand[0].value).Count == 3 || hand.FindAll(c => c.value == hand[1].value).Count == 3 || hand.FindAll(c => c.value == hand[2].value).Count == 3)
			handValue = PokerHandValue.ThreeOfAKind;
		// if two pair
		else if (false)
			handValue = PokerHandValue.TwoPair;
		// if pair
		else if (false)
			handValue = PokerHandValue.Pair;
		// else
		else
			handValue = PokerHandValue.HighCard;

		return (int)handValue;
	}

	/// <summary>
	/// Resets the deck to a fresh full randomized deck from the cards list.
	/// </summary>
	public void Shuffle()
	{
		deck.Clear();

		List<Card> tempCards = new List<Card>(cards);

		for (int i = 0; i < cards.Count; i++)
		{
			int cardIndex = UnityEngine.Random.Range(0, tempCards.Count);
			Card card = tempCards[cardIndex];
			card.isFaceUp = true;
			deck.Add(card);
			tempCards.RemoveAt(cardIndex);
		}
	}
}
