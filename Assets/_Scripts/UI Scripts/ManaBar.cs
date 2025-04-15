using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{

    public GameObject tickPrefab; //white dividing lines
    public GameObject tickHolder; //holds all your ticks

    private RectTransform thisTransform;
    private float fullWidth;

    private int maxValue;
    private int currentValue;

    // Start is called before the first frame update
    void Awake()
    {
        fullWidth = this.GetComponent<RectTransform>().sizeDelta.x;
        thisTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaxMana(int mana)
    {
        maxValue = mana;
        currentValue = mana;

        //set the new width to the specified fraction (without ever updating fullWidth cuz I need that to know the baseline size)
        Vector2 size = thisTransform.sizeDelta;
        size.x = fullWidth * ((float)currentValue / maxValue);
        thisTransform.sizeDelta = size;

        ClearTickMarks();
        CreateTickMarks();
    }

    public void SetMana(int mana)
    {
        currentValue = mana;

        Vector2 size = thisTransform.sizeDelta;
        size.x = fullWidth * ((float)currentValue / maxValue);
        thisTransform.sizeDelta = size;
    }

    void CreateTickMarks()
    {
        float tickSpacing = fullWidth / maxValue;

        for (int i = 1; i < maxValue; i++) // skip 0% and 100% bar fill
        {
            GameObject tickMark = Instantiate(tickPrefab, tickHolder.transform);

            RectTransform tickTransform = tickMark.GetComponent<RectTransform>();
            tickTransform.anchorMin = new Vector2(0, 0.5f);
            tickTransform.anchorMax = new Vector2(0, 0.5f);
            tickTransform.pivot = new Vector2(0.5f, 0.5f);

            //Position tick along the bar
            float x = -tickSpacing * i;
            tickTransform.anchoredPosition = new Vector2(x, 0);
        }
    }

    void ClearTickMarks()
    {
        var ticks = GameObject.FindGameObjectsWithTag("ManaTick");
        foreach (GameObject tick in ticks)
        {
            Destroy(tick);
        }
    }
}