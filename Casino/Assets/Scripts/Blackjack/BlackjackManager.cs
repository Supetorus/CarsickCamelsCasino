using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackjackManager : MonoBehaviour
{
    public PlayerInfo playerInfo;

    public TMP_Text playerAvailableChips;

    public TMP_Text playerAvailableBalance;

    public TMP_Text playerBetValue;
    public TMP_Text playerSplitBetValue;

    /// <summary>
    /// Prefabs for each card in a deck.
    /// </summary>
    public List<Card> cards = new List<Card>();

    /// <summary>
    /// The parent gameobject where player cards should be instantiated.
    /// </summary>
    public GameObject playerHandLocation;
    public GameObject playerSplitHandLocation;

    /// <summary>
    /// The text display for the player's hand value.
    /// </summary>
    public TMP_Text playerHandText;
    public TMP_Text playerSplitHandText;

    /// <summary>
    /// The text display for the dealer's hand value.
    /// </summary>
    public TMP_Text dealerHandText;

    /// <summary>
    /// The parent gameobject where dealer cards should be instantiated.
    /// </summary>
    public GameObject dealerHandLocation;

    /// <summary>
    /// The sprite shown for a card that is face down.
    /// </summary>
    public Sprite faceDownCard;

    public GameObject gameEndPanel;
    public GameObject splitButton;
    public GameObject doubleButton;

    public int minBet = 2;

    private int playerBet = 0;
    private int playerSplitBet = 0;

    /// <summary>
    /// A deck of prefab cards which have not been dealt.
    /// </summary>
    private List<Card> deck = new List<Card>();

    /// <summary>
    /// The cards in the dealer's hand
    /// </summary>
    List<Card> dealerHand = new List<Card>();

    /// <summary>
    /// The cards in the player's hand
    /// </summary>
    List<Card> playerHand = new List<Card>();
    List<Card> playerSplitHand = new List<Card>();

    bool splitHand = false;

    int playerScore = 0;
    int playerSplitScore = 0;
    int dealerScore = 0;

    private enum GameState
    {
        Betting,
        Playing,
        GameOver,
    }

    private GameState gameState;

    private void Start()
    {
        playerAvailableChips.text = playerInfo.chipBalance.ToString();
        playerAvailableBalance.text = playerInfo.bankBalance.ToString();

        gameState = GameState.Betting;
        gameEndPanel.SetActive(false);
    }

    /// <summary>
    /// Called by deal button. Shuffles the deck and deals two cards to player and dealer. Dealer has one card face down.
    /// </summary>
    public void Deal()
    {
        if (gameState != GameState.Betting || playerBet < minBet) return;
        gameState = GameState.Playing;
        Shuffle();
        playerHand.Clear();
        dealerHand.Clear();

        foreach (Transform card in playerHandLocation.transform)
        {
            Destroy(card.gameObject);
        }

        playerHand.Add(cards[38]);
        playerHand.Add(cards[39]);

        for (int i = 0; i < 2; i++)
        {
            //playerHand.Add(GetCard());
            dealerHand.Add(GetCard());
        }

        if (playerInfo.chipBalance < playerBet)
        {
            splitButton.SetActive(false);
            doubleButton.SetActive(false);
        }

        if (playerHand[0].value != playerHand[1].value)
        {
            splitButton.SetActive(false);
        }

        dealerHand[0].isFaceUp = false;

        CheckBlackjack();

        DisplayCards();
    }

    /// <summary>
    /// Doubles the current bet, and deals a card to the player, then the player stands
    /// Option is not available if chip balance is less than current bet
    /// </summary>
    public void DoubleDown()
    {
        playerInfo.chipBalance -= playerBet;
        if (splitHand)
        {
            playerSplitBet *= 2;
            playerSplitHand.Add(GetCard());
        }
        else
        {
            playerBet *= 2;
            playerHand.Add(GetCard());
        }
        PlayerStand();
    }

    /// <summary>
    /// Splits the player's hand into 2, places the current bet on the new hand, and deals a card to each hand
    /// Option is not available if chip balance is less than current bet
    /// </summary>
    public void Split()
    {
        playerSplitHand.Add(playerHand[1]);
        playerHand.RemoveAt(1);

        playerInfo.chipBalance -= playerBet;
        playerSplitBet = playerBet;

        playerHand.Add(GetCard());
        playerSplitHand.Add(GetCard());
        DisplayCards();
        CheckBlackjack();

        splitHand = true;

        if (playerInfo.chipBalance < playerBet)
        {
            doubleButton.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the card displays and value display
    /// </summary>
    public void DisplayCards()
    {
        if (playerHand.Count < 0) return;

        // Player hand
        playerScore = CalculateHandValue(playerHand);
        playerHandText.text = playerScore.ToString();

        if (playerScore == 21) playerHandText.color = Color.green;
        else if (playerScore > 21) playerHandText.color = Color.red;
        else playerHandText.color = Color.white;

        // Player's Split hand
        playerSplitScore = CalculateHandValue(playerSplitHand);
        playerSplitHandText.text = playerSplitScore.ToString();

        if (playerSplitScore == 21) playerSplitHandText.color = Color.green;
        else if (playerSplitScore > 21) playerSplitHandText.color = Color.red;
        else playerSplitHandText.color = Color.white;

        // Dealer's hand
        dealerScore = CalculateHandValue(dealerHand);
        dealerHandText.text = dealerScore == 0 ? "" : dealerScore.ToString();

        if (dealerScore == 21) dealerHandText.color = Color.green;
        else if (dealerScore > 21) dealerHandText.color = Color.red;
        else dealerHandText.color = Color.white;

        foreach (Transform c in playerHandLocation.transform) Destroy(c.gameObject);
        foreach (Transform c in playerSplitHandLocation.transform) Destroy(c.gameObject);
        foreach (Transform c in dealerHandLocation.transform) Destroy(c.gameObject);
        foreach (Card c in playerHand) Instantiate(c.gameObject, playerHandLocation.transform);
        foreach (Card c in playerSplitHand) Instantiate(c.gameObject, playerSplitHandLocation.transform);
        foreach (Card c in dealerHand)
        {
            Card newCard = Instantiate(c.gameObject, dealerHandLocation.transform).GetComponent<Card>();
            if (newCard.isFaceUp) newCard.GetComponent<Image>().sprite = newCard.defaultSprite;
            else newCard.GetComponent<Image>().sprite = faceDownCard;
        }
    }

    /// <summary>
    /// Updates the chips displays.
    /// </summary>
    public void DisplayChips()
    {
        playerBetValue.text = playerBet.ToString();
        playerAvailableChips.text = playerInfo.chipBalance.ToString();
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
    public void PlayerHit()
    {
        if (gameState != GameState.Playing) return;

        if (splitHand)
        {
            playerSplitHand.Add(GetCard());
            DisplayCards();
            if (CalculateHandValue(playerSplitHand) >= 21)
            {
                if (playerInfo.chipBalance >= playerBet && playerInfo.chipBalance >= minBet)
                {
                    doubleButton.SetActive(true);
                }
                splitHand = false;
                return;
            }
        }

        playerHand.Add(GetCard());
        if (CalculateHandValue(playerHand) >= 21) StartCoroutine(GameOver());
        DisplayCards();
    }

    private void CheckBlackjack()
    {
        if (CalculateHandValue(playerSplitHand) == 21)
        {
            splitHand = false;
        }

        if (CalculateHandValue(playerHand) == 21)
        {
            dealerHand[0].isFaceUp = true;
            StartCoroutine(GameOver());
        }
    }

    public void Reset()
    {
        playerHand.Clear();
        playerSplitHand.Clear();
        dealerHand.Clear();
        playerBet = 0;
        playerSplitBet = 0;

        DisplayCards();
        DisplayChips();
        gameState = GameState.Betting;
    }

    /// <summary>
    /// The method called when player clicks "Stand" button.
    /// </summary>
    public void PlayerStand()
    {
        if (gameState != GameState.Playing) return;

        if (splitHand)
        {
            if (playerInfo.chipBalance >= playerBet && playerInfo.chipBalance >= minBet)
            {
                doubleButton.SetActive(true);
            }
            splitHand = false;
        }
        else
        {
            DealerPlay();
        }
    }

    /// <summary>
    /// Calculates and returns the value of the hand given. If ace as 11 puts the value
    /// over 21 then it is changed to 1.
    /// If card is face down it is skipped.
    /// </summary>
    public int CalculateHandValue(List<Card> hand)
    {
        int aces = 0;
        int result = 0;

        foreach (Card card in hand)
        {
            if (!card.isFaceUp) continue;
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
        if (gameState != GameState.Betting || playerInfo.chipBalance < bet) return;
        if (playerInfo.chipBalance > bet)
        {
            playerBet += bet;
            playerInfo.chipBalance -= bet;
            DisplayChips();
        }
    }

    public void DealerPlay()
    {
        gameState = GameState.GameOver;
        dealerHand[0].isFaceUp = true;
        playerScore = CalculateHandValue(playerHand);
        dealerScore = CalculateHandValue(dealerHand);
        while (dealerScore < 17 || dealerScore < playerScore)
        {
            dealerHand.Add(GetCard());
            dealerScore = CalculateHandValue(dealerHand);
        }

        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        playerScore = CalculateHandValue(playerHand);
        playerSplitScore = CalculateHandValue(playerSplitHand);
        dealerScore = CalculateHandValue(dealerHand);

        if (playerScore == 21) // player has 21
        {
            if (dealerScore == 21)
            {
                if (playerHand.Count == 2 && dealerHand.Count != 2)
                {
                    playerInfo.chipBalance += playerBet * 3;
                }
                else
                {
                    playerInfo.chipBalance += playerBet;
                }
            }
            else playerInfo.chipBalance += playerBet * 2;
        }
        else if (dealerScore > 21)
        {
            playerInfo.chipBalance += playerBet * 2;
        }
        else if (dealerScore < 21 && playerScore < 21)
        {
            if (playerScore > dealerScore) playerInfo.chipBalance += playerBet * 2;
            else if (dealerScore == playerScore) playerInfo.chipBalance += playerBet;
        }

        DisplayCards();
        DisplayChips();

        yield return new WaitForSeconds(2);

        gameEndPanel.SetActive(true);
    }
}
