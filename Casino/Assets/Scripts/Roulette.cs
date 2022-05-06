using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
	[SerializeField] Sprite[] chips;
	int selectedChip = 0;
	RButton[] buttons;

	void Start()
	{
		GetComponentInChildren<GridLayoutGroup>().enabled = false;

		buttons = GetComponentsInChildren<RButton>();
	}
}
