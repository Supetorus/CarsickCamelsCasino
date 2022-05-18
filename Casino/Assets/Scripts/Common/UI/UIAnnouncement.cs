using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAnnouncement : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] float waitTimer = 0;

    public void Display(string text)
    {
        label.text = text;
        Color color = label.color;
        color.a = 1;
        waitTimer = 1;
        label.color = color;

    }

    void Update()
    {
        Color color = label.color;

        if ( color.a  >  0)
        {
            if (waitTimer > 0)
            {
                waitTimer -= Time.deltaTime;
            }
            else
            {
                color.a -= 1 * Time.deltaTime / 2;
            }
        }
        Debug.Log(color.a);
        label.color = color;
    }
}

