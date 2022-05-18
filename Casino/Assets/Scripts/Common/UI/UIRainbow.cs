using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRainbow : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    private int speed = 1;
    void Update()
    {
        float h, s, v;
        Color.RGBToHSV(label.color, out h, out s, out v);

        Color beans = Color.HSVToRGB(h + Time.deltaTime * .25f, s, v);
        Debug.Log(beans.ToString());
        label.color = beans;
    }
}
