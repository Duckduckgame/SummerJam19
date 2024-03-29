﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class tickerController : MonoBehaviour
{
    public TextMeshProUGUI sunText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI nutrientText;

    public TextMeshProUGUI sunPerTickText;
    public TextMeshProUGUI waterPerTickText;
    public TextMeshProUGUI nutrientPerTickText;

    public float sunAmount = 50;
    public float waterAmount = 0;
    public float nutrientAmount = 0;

    public float sunTickAmount = 0;
    public float waterTickAmount = 0;
    public float nutrientTickAmount = 0;

    public int sunFlower01Amount = 1;

    public int tickerCooldown = 1;

    public List<GameObject> flowers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        sunAmount = 150;

        InvokeRepeating("tickFlowers", 0, tickerCooldown);

        sunText = GameObject.Find("sunText").GetComponent<TextMeshProUGUI>();
        waterText = GameObject.Find("waterText").GetComponent<TextMeshProUGUI>();
        nutrientText = GameObject.Find("nutrientText").GetComponent<TextMeshProUGUI>();

        sunPerTickText = GameObject.Find("sunPerTickText").GetComponent<TextMeshProUGUI>();
        waterPerTickText = GameObject.Find("waterPerTickText").GetComponent<TextMeshProUGUI>();
        nutrientPerTickText = GameObject.Find("nutrientPerTickText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        sunText.text = "Sun: " + Mathf.RoundToInt(sunAmount).ToString();
        waterText.text = "Water: " + Mathf.RoundToInt(waterAmount).ToString();
        nutrientText.text = "Nutrients: " + Mathf.RoundToInt(nutrientAmount).ToString();

        if (Input.GetKeyDown(KeyCode.RightControl)) {
            sunAmount += 1000;
            nutrientAmount += 1000;
            waterAmount += 1000;
        }
    }

    void tickFlowers() {

        sunTickAmount = 0;
        waterTickAmount = 0;
        nutrientTickAmount = 0;

        foreach (GameObject flower in flowers)
        {
            flowerInfo FI = flower.GetComponent<flowerInfo>();
            
            sunTickAmount += FI.crntSunPerTick;
            waterTickAmount += FI.crntWaterPerTick * FI.localWaterAmount;
            nutrientTickAmount += FI.crntNutrientPerTick * FI.localNutrientAmount;
            
        }
        sunAmount += sunTickAmount;
        waterAmount += waterTickAmount;
        nutrientAmount += nutrientTickAmount;

        sunPerTickText.text = "Per Minute: " + (sunTickAmount*60).ToString("F2");
        nutrientPerTickText.text = "Per Minute: " + (nutrientTickAmount*60).ToString("F2");
        waterPerTickText.text = "Per Minute: " + (waterTickAmount*60).ToString("F2");
    }
}
