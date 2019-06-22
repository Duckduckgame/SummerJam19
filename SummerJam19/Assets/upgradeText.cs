using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class upgradeText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI TMP;
    public GameObject flower;
    flowerInfo FI;
    Button but;
    tickerController TC;
    tileScript TS;

    private void Start()
    {
        but = GetComponent<Button>();
        TMP = GameObject.Find("Cost Text").GetComponent<TextMeshProUGUI>();

        TC = GameObject.Find("TickerController").GetComponent<tickerController>();
        TS = GameObject.Find("TileManager").GetComponent<tileScript>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FI = TS.mapToFlower[TS.selectedTile];
        TMP.transform.position = Input.mousePosition + new Vector3(150, 0, 0);
        if (FI.crntUpgradeLvl == flowerInfo.upgradeLevel.one)
        {
            TMP.text = "Sun: " + FI.u1SunCost + " Water: " + FI.u1WaterCost + " Nutrient " + FI.u1NutrientCost;
        }
        if (FI.crntUpgradeLvl == flowerInfo.upgradeLevel.two)
        {
            TMP.text = "Sun: " + FI.u2SunCost + " Water: " + FI.u2WaterCost + " Nutrient " + FI.u2NutrientCost;
        }


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TMP.transform.position = new Vector3(-500, 0, 0);
    }

    private void Update()
    {
        if (TS.selectedTile != null)
        {
            try
            {
                FI = TS.mapToFlower[TS.selectedTile];

                if (FI.crntUpgradeLvl == flowerInfo.upgradeLevel.one)
                {
                    if (TC.sunAmount >= FI.u1SunCost && TC.waterAmount >= FI.u1WaterCost && TC.nutrientAmount >= FI.u1NutrientCost)
                        but.interactable = true;
                    if (TC.sunAmount < FI.u1SunCost || TC.waterAmount < FI.u1WaterCost || TC.nutrientAmount < FI.u1NutrientCost)
                        but.interactable = false;
                }
                if (FI.crntUpgradeLvl == flowerInfo.upgradeLevel.two)
                {
                    if (TC.sunAmount >= FI.u2SunCost && TC.waterAmount >= FI.u2WaterCost && TC.nutrientAmount >= FI.u2NutrientCost)
                        but.interactable = true;
                    if (TC.sunAmount < FI.u2SunCost || TC.waterAmount < FI.u2WaterCost || TC.nutrientAmount < FI.u2NutrientCost)
                        but.interactable = false;
                }
            }
            catch { }

        }
    }
}
