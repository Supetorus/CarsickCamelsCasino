using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RButton : MonoBehaviour
{
	static Vector2 chipSize = new Vector2(100, 100);
	Vector2 size;
	Button button;
	Image image;

	RectTransform rect;

	void Start()
	{
		button = GetComponent<Button>();
		image = GetComponent<Image>();
		rect = GetComponent<RectTransform>();
		size = rect.sizeDelta;
	}

	public void SetChip(bool set, Sprite chip)
	{
		if(set)
		{
			rect.sizeDelta = chipSize;
			image.sprite = chip;
			image.color = Color.white;
		}
		else
		{
			rect.sizeDelta = size;
			image.sprite = null;
			image.color = Color.clear;
		}
	}
}
