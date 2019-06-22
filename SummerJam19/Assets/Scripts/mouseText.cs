using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class mouseText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI TMP;
    public GameObject flower;
    flowerInfo FI;
    Button but;
    tickerController TC;

    private void Start()
    {
        but = GetComponent<Button>();
        TMP = GameObject.Find("Cost Text").GetComponent<TextMeshProUGUI>();
        FI = flower.GetComponent<flowerInfo>();
        TC = GameObject.Find("TickerController").GetComponent<tickerController>();
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        
        TMP.transform.position = Input.mousePosition + new Vector3(150, 0, 0);
        TMP.text = "Sun: " + FI.baseSunCost + " Water: " + FI.baseWaterCost + " Nutrient " + FI.baseNutrientCost;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TMP.transform.position = new Vector3(-500, 0, 0);
    }

    private void Update()
    {
        if (TC.sunAmount >= FI.baseSunCost && TC.waterAmount >= FI.baseWaterCost && TC.nutrientAmount >= FI.baseNutrientCost) {
            but.interactable = true;
        }

        if (TC.sunAmount < FI.baseSunCost || TC.waterAmount < FI.baseWaterCost || TC.nutrientAmount < FI.baseNutrientCost) {
            but.interactable = false;
        }
    }
}
