using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlinkoManager : MonoBehaviour
{
    public PlayerInfo playerInfo;

    public TMP_Text playerAvailableChips;

    public TMP_Text playerAvailableBalance;

    public TMP_Text playerBetValue;

    public GameObject gameEndPanel;

    public int minBet = 2;

    private int playerBet = 0;

    private enum GameState
    {
        Betting,
        Playing,
        GameOver,
    }

    private GameState gameState;

    // Update is called once per frame
    void Update()
    {
        
    }
}
