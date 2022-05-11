using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct Bet
{
	public Bet(int multi, params int[] validNums)
	{
		multiplier = multi;
		numbers = validNums;
	}

	public readonly int multiplier;
	public readonly int[] numbers;
}

public class Roulette : MonoBehaviour
{
	/* TODO:
	 * Chip highlighting
	 * Multiple bets
	 * Display total bet amount
	 * 'Roll' for a number
	 * Display rolled number
	 * Use bank system
	 */

	[SerializeField] PlayerInfo playerInfo;
	[SerializeField] Sprite[] chips;
	[SerializeField] GameObject chipPanel;
	RButton[] buttons;
	private int selectedChip = 0;
	private int selectedCell = -1;
	private bool needsLayout = true;

	private readonly int[] betAmts = { 1, 5, 10, 20, 50, 100, 500, 1000, 5000 };

	private readonly int[] betMultiplier = { 35, 17, 11, 8, 5, 2, 1 };

	private readonly Bet[] bets =
	{
		new Bet(5, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12),
		new Bet(5, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24),
		new Bet(5, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36),
		new Bet(6, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18),
		new Bet(6, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36),
		new Bet(6, 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36),
		new Bet(6, 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35),
		new Bet(6, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35),
		new Bet(6, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36),
		new Bet(0, 3), //9
		new Bet(1, 3, 6),
		new Bet(0, 6),
		new Bet(1, 6, 9),
		new Bet(0, 9),
		new Bet(1, 9, 12),
		new Bet(0, 12),
		new Bet(1, 12, 15),
		new Bet(0, 15),
		new Bet(1, 15, 18),
		new Bet(0, 18),
		new Bet(1, 18, 21),
		new Bet(0, 21),
		new Bet(1, 21, 24),
		new Bet(0, 24),
		new Bet(1, 24, 27),
		new Bet(0, 27),
		new Bet(1, 27, 30),
		new Bet(0, 30),
		new Bet(1, 30, 33),
		new Bet(0, 33),
		new Bet(1, 33, 36),
		new Bet(0, 36),
		new Bet(1, 2, 3), //32
		new Bet(3, 2, 3, 5, 6),
		new Bet(1, 5, 6),
		new Bet(3, 5, 6, 8, 9),
		new Bet(1, 8, 9),
		new Bet(3, 8, 9, 11, 12),
		new Bet(1, 11, 12),
		new Bet(3, 11, 12, 14, 15),
		new Bet(1, 14, 15),
		new Bet(3, 14, 15, 17, 18),
		new Bet(1, 17, 18),
		new Bet(3, 17, 18, 20, 21),
		new Bet(1, 20, 21),
		new Bet(3, 20, 21, 23, 24),
		new Bet(1, 23, 24),
		new Bet(3, 23, 24, 26, 27),
		new Bet(1, 26, 27),
		new Bet(3, 26, 27, 29, 30),
		new Bet(1, 29, 30),
		new Bet(3, 29, 30, 32, 33),
		new Bet(1, 32, 33),
		new Bet(3, 32, 33, 35, 36),
		new Bet(1, 35, 36),
		new Bet(0, 2), //55
		new Bet(1, 2, 5),
		new Bet(0, 5),
		new Bet(1, 5, 8),
		new Bet(0, 8),
		new Bet(1, 8, 11),
		new Bet(0, 11),
		new Bet(1, 11, 14),
		new Bet(0, 14),
		new Bet(1, 14, 17),
		new Bet(0, 17),
		new Bet(1, 17, 20),
		new Bet(0, 20),
		new Bet(1, 20, 23),
		new Bet(0, 23),
		new Bet(1, 23, 26),
		new Bet(0, 26),
		new Bet(1, 26, 29),
		new Bet(0, 29),
		new Bet(1, 29, 32),
		new Bet(0, 32),
		new Bet(1, 32, 35),
		new Bet(0, 35),
		new Bet(1, 1, 2), //78
		new Bet(3, 1, 2, 4, 5),
		new Bet(1, 4, 5),
		new Bet(3, 4, 5, 7, 8),
		new Bet(1, 7, 8),
		new Bet(3, 7, 8, 10, 11),
		new Bet(1, 10, 11),
		new Bet(3, 10, 11, 13, 14),
		new Bet(1, 13, 14),
		new Bet(3, 13, 14, 16, 17),
		new Bet(1, 16, 17),
		new Bet(3, 16, 17, 19, 20),
		new Bet(1, 19, 20),
		new Bet(3, 19, 20, 22, 23),
		new Bet(1, 22, 23),
		new Bet(3, 22, 23, 25, 26),
		new Bet(1, 25, 26),
		new Bet(3, 25, 26, 28, 29),
		new Bet(1, 28, 29),
		new Bet(3, 28, 29, 31, 32),
		new Bet(1, 31, 32),
		new Bet(3, 31, 32, 34, 35),
		new Bet(1, 34, 35),
		new Bet(0, 1), //101
		new Bet(1, 1, 4),
		new Bet(0, 4),
		new Bet(1, 4, 7),
		new Bet(0, 7),
		new Bet(1, 7, 10),
		new Bet(0, 10),
		new Bet(1, 10, 13),
		new Bet(0, 13),
		new Bet(1, 13, 16),
		new Bet(0, 16),
		new Bet(1, 16, 19),
		new Bet(0, 19),
		new Bet(1, 19, 22),
		new Bet(0, 22),
		new Bet(1, 22, 25),
		new Bet(0, 25),
		new Bet(1, 25, 28),
		new Bet(0, 28),
		new Bet(1, 28, 31),
		new Bet(0, 31),
		new Bet(1, 31, 34),
		new Bet(0, 34),
		new Bet(2, 1, 2, 3), //124
		new Bet(4, 1, 2, 3, 4, 5, 6),
		new Bet(2, 4, 5, 6),
		new Bet(4, 4, 5, 6, 7, 8, 9),
		new Bet(2, 7, 8, 9),
		new Bet(4, 7, 8, 9, 10, 11, 12),
		new Bet(2, 10, 11, 12),
		new Bet(4, 10, 11, 12, 13, 14, 15),
		new Bet(2, 13, 14, 15),
		new Bet(4, 13, 14, 15, 16, 17, 18),
		new Bet(2, 16, 17, 18),
		new Bet(4, 16, 17, 18, 19, 20, 21),
		new Bet(2, 19, 20, 21),
		new Bet(4, 19, 20, 21, 22, 23, 24),
		new Bet(2, 22, 23, 24),
		new Bet(4, 22, 23, 24, 25, 26, 27),
		new Bet(2, 25, 26, 27),
		new Bet(4, 25, 26, 27, 28, 29, 30),
		new Bet(2, 28, 29, 30),
		new Bet(4, 28, 29, 30, 31, 32, 33),
		new Bet(2, 31, 32, 33),
		new Bet(4, 31, 32, 33, 34, 35, 36),
		new Bet(2, 34, 35, 36),
		new Bet(5, 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36), //147
		new Bet(5, 2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35),
		new Bet(5, 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34),
		new Bet(0, 0), //150
		new Bet(2, 0, 2, 3),
		new Bet(2, 0, 1, 2),
		new Bet(3, 0, 1, 2, 3),
	};

	void Awake()
	{
		buttons = GetComponentsInChildren<RButton>();

		int c = 0;
		foreach(var chip in chipPanel.GetComponentsInChildren<Button>())
		{
			int temp = c;
			chip.onClick.AddListener(() => SetChip(temp));
			++c;
		}

		for(int i = 0; i < buttons.Length; ++i)
		{
			int temp = i;
			buttons[i].GetComponent<Button>().GetComponent<Button>().onClick.AddListener(() => ClickCell(temp));
		}
	}

	public void SetChip(int i)
	{
		//TODO: Highlighting
		selectedChip = i;
	}

	public void ClickCell(int i)
	{
		//TODO: Multiple bets

		if(needsLayout)
		{
			GetComponentInChildren<GridLayoutGroup>().enabled = false;
			needsLayout = false;
		}

		if(selectedCell > -1)
		{
			buttons[selectedCell].SetChip(false, null);
		}

		selectedCell = i;
		buttons[i].SetChip(true, chips[selectedChip]);
	}

	public void Spin()
	{
		
	}
}
