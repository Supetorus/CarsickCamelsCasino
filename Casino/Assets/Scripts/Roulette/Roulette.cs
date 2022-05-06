using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
	[SerializeField] Sprite[] chips;
	[SerializeField] GameObject chipPanel;
	RButton[] buttons;
	private int selectedChip = 0;
	private int selectedCell = -1;
	private bool needsLayout = true;

	private readonly int[] betAmts = { 1, 5, 10, 20, 50, 100, 500, 1000, 5000 };

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
		selectedChip = i;
	}

	public void ClickCell(int i)
	{
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
}
