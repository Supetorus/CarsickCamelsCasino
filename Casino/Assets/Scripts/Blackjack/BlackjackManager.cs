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

	/// <summary>
    /// Prefabs for each card in a deck.
    /// </summary>
	public List<Card> cards = new List<Card>();

    /// <summary>
    /// The parent gameobject where player cards should be instantiated.
    /// </summary>
    public GameObject playerHandLocation;

    /// <summary>
    /// The text display for the player's hand value.
    /// </summary>
    public TMP_Text playerHandText;

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

    private int playerBet = 0;

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
        if (gameState != GameState.Betting) return;
        gameState = GameState.Playing;
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

        dealerHand[0].isFaceUp = false;

        CheckBlackjack();

        DisplayCards();
    }

    /// <summary>
    /// Updates the card displays and value display
    /// </summary>
    public void DisplayCards()
	{
        if (playerHand.Count < 0) return;

        int playerHandValued = CalculateHandValue(playerHand);
        playerHandText.text = playerHandValued.ToString();

        if (playerHandValued == 21) playerHandText.color = Color.green;
        else if (playerHandValued > 21) playerHandText.color = Color.red;
        else playerHandText.color = Color.white;

        int dealerHandValued = CalculateHandValue(dealerHand);
        dealerHandText.text = dealerHandValued == 0 ? "" : dealerHandValued.ToString();

        if (dealerHandValued == 21) dealerHandText.color = Color.green;
        else if (dealerHandValued > 21) dealerHandText.color = Color.red;
        else dealerHandText.color = Color.white;

        foreach (Transform c in playerHandLocation.transform) Destroy(c.gameObject);
        foreach (Transform c in dealerHandLocation.transform) Destroy(c.gameObject);
        foreach (Card c in playerHand) Instantiate(c.gameObject, playerHandLocation.transform);
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

        playerHand.Add(GetCard());
        if (CalculateHandValue(playerHand) > 21) StartCoroutine(GameOver());
        CheckBlackjack();
        DisplayCards();
	}

    private void CheckBlackjack()
	{
        if (CalculateHandValue(playerHand) == 21)
		{
            StartCoroutine(GameOver());
		}
	}

	public void Reset()
	{
        for (int i = 0; i < playerHandLocation.transform.childCount; i++)
        {
            Destroy(playerHandLocation.transform.GetChild(i).gameObject);
            print("Destroying: " + playerHandLocation.transform.GetChild(i).gameObject.name);
        }
        for (int i = 0; i < dealerHandLocation.transform.childCount; i++)
        {
            Destroy(dealerHandLocation.transform.GetChild(i).gameObject);
            print("Destroying: " + dealerHandLocation.transform.GetChild(i).gameObject.name);
        }
        playerBet = 0;

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

        DealerPlay();
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

        foreach(Card card in hand)
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
            card.isFaceUp = true;
            deck.Add(card);
            tempCards.Remove(card);
        }
    }

    public void AddBet(int bet)
	{
        if (gameState != GameState.Betting) return;
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
        int playerScore = CalculateHandValue(playerHand);
        int dealerScore = CalculateHandValue(dealerHand);
        while (dealerScore < 17 || dealerScore < playerScore)
		{
            dealerHand.Add(GetCard());
		}

        StartCoroutine(GameOver());
	}

    private IEnumerator GameOver()
	{
        int playerScore = CalculateHandValue(playerHand);
        int dealerScore = CalculateHandValue(dealerHand);

        if (playerScore == 21 && dealerScore != 21)
		{
            playerInfo.chipBalance += playerBet * 3;
		}
        if (playerScore > dealerScore && playerScore <= 21)
        {
            playerInfo.chipBalance += playerBet * 2;
        }
        else if (playerScore == dealerScore && playerScore <= 21)
        {
            playerInfo.chipBalance += playerBet;
        }
        DisplayCards();

        yield return new WaitForSeconds(2);

        gameEndPanel.SetActive(true);
	}
}
