using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokerLogic : MonoBehaviour
{

    /*
     * What the fridge needs the be done?
     * -Get player's money count
     * -Check if bet is valid
     * -Let player "Hold" cards
     * -Regenerate non-held cards
     * -Check for win
     * IF(win = payout/reset, lost = reset, lost + no money = main menu)
     */

    /// <summary>
    /// Created variables for the on screen text to be set
    /// </summary>
    public TMP_Text playerMoneyCount;
    public TMP_Text playerBetAmount;
    public TMP_Text playerAnnouncement;

    /// <summary>
    /// The parent gameobject where player cards should be instantiated.
    /// </summary>
    public GameObject playerHandLocation;

    /// <summary>
    /// Generate prefabs for the cards and a deck to be pulled from
    /// </summary>
    public List<Card> cards = new List<Card>();
    private List<Card> deck = new List<Card>();

    /// <summary>
    /// The cards in the player's hand and bet amount
    /// </summary>
    List<Card> playerHand = new List<Card>();
    private int playerBet = 0;

    /// <summary>
    /// Shuffles the deck and deals five cards to player
    /// </summary>
    public void Deal()
    {
        Shuffle();
        playerHand.Clear();

        foreach (Transform card in playerHandLocation.transform)
        {
            Destroy(card.gameObject);
        }

        for (int i = 0; i < 5; i++)
        {
            playerHand.Add(GetCard());
        }

        Display();
    }

    /// <summary>
    /// Updates the card displays and value display
    /// </summary>
    public void Display()
    {

        playerBetAmount.text = playerBet.ToString();

        if (playerHand.Count < 0) return;

        foreach (Transform c in playerHandLocation.transform) Destroy(c.gameObject);
        foreach (Card c in playerHand) Instantiate(c.gameObject, playerHandLocation.transform);
        
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
    /// Resets the deck to a fresh full randomized deck.
    /// </summary>
    public void Shuffle()
    {
        deck.Clear();

        List<Card> tempCards = new List<Card>(cards);

        for (int i = 0; i < cards.Count; i++)
        {
            Card card = tempCards[Random.Range(0, tempCards.Count)];
            card.isFaceUp = true;
            deck.Add(card);
            tempCards.Remove(card);
        }
    }

    public void AddBet(int bet)
    {
        playerBet += bet;
        Display();
    }
}
