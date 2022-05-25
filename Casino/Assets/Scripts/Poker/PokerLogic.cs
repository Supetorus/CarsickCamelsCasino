using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokerLogic : MonoBehaviour
{
    public CardManager cardManager;
    public PlayerInfo playerInfo;
    public List<Card> playerDebugHand;

    [Header("Displays")]
    public TMP_Text playerAvailableChips;
    public TMP_Text playerAvailableBalance;
    public TMP_Text playerBetValue;
    private List<Card> playerHand = new List<Card>();

    [Header("Locations")]
    public GameObject playerHandLocation;

    private int playerBet = 0;

    private enum GameState
    {
        Betting,
        FirstDraw,
        Redraw,
        GameOver
    }

    private GameState gameState;

    private void Start()
    {
        playerAvailableChips.text = playerInfo.chipBalance.ToString();
        playerAvailableBalance.text = playerInfo.bankBalance.ToString();

        gameState = GameState.Betting;
    }

    /// <summary>
    /// Shuffles the deck and deals five cards to player. Called when the "Bet" button is clicked
    /// </summary>
    public void Deal()
    {
        if (gameState != GameState.Betting) return;
        gameState = GameState.FirstDraw;
        cardManager.Shuffle();
        cardManager.Deal(5, true, playerHand);

        foreach (Transform card in playerHandLocation.transform) Destroy(card.gameObject);

        if (playerDebugHand != null) playerHand = playerDebugHand;

        DisplayCards();
    }

    /// <summary>
    /// Updates the card displays and value display
    /// </summary>
    public void DisplayCards()
    {
        if (playerHand.Count < 0) return;

        //IMPLEMENT WINS WITH AUSTIN'S CODE ON CARDMANAGER
        //playerScore = cardManager.CalculateHandValue(playerHand, CardManager.CardRules.Blackjack);
        //playerHandText.text = playerScore.ToString();

        foreach (Transform c in playerHandLocation.transform) Destroy(c.gameObject);
        foreach (Card c in playerHand) Instantiate(c.gameObject, playerHandLocation.transform);
    }

    public void DisplayMoney()
    {
        playerBetValue.text = playerBet.ToString();
        playerAvailableChips.text = playerInfo.chipBalance.ToString();
        playerAvailableBalance.text = playerInfo.bankBalance.ToString();
    }

    public void ResetGame()
    {
        playerHand.Clear();
        playerBet = 0;

        DisplayCards();
        DisplayMoney();
        gameState = GameState.Betting;
    }

    private IEnumerator GameOver()
    {
        //int playerScore = cardManager.CalculateHandValue(playerHand, CardManager.CardRules.Blackjack);

        /* If (Player wins) 
         * {
         *      Multiply their bet by type of win and return chips to player
         * }
         * If (Player loses)
         * {
         *      Make bet zero and and don't return anything
         * }
         */

        DisplayCards();
        DisplayMoney();

        yield return new WaitForSeconds(2);

    }

    public void AddBet(int bet)
    {
        if (gameState != GameState.Betting || playerInfo.chipBalance < bet) return;
        if (playerInfo.chipBalance > bet)
        {
            playerBet += bet;
            playerInfo.chipBalance -= bet;
            DisplayMoney();
        }
    }
}
