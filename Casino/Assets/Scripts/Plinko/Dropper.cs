using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] GameObject highlight;
    [SerializeField] GameObject ballPrefab;

    public bool ballActive;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnMouseDown()
    {
        Instantiate(ballPrefab, highlight.transform.position, highlight.transform.rotation);
    }

    private void OnMouseOver()
    {
        if (!ballActive)
        {
            Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mPos.z = 0;
            mPos.y = 5.25f;
            mPos.x = Mathf.Clamp(mPos.x, -3.3f, 8.3f);
            //Debug.Log(mPos);
            highlight.transform.position = mPos;
            //Debug.Log(highlight.transform.position);
        }
    }
}
