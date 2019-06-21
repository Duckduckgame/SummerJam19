using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class mouseText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI TMP;
    public GameObject flower;
    flowerInfo FI; 

    private void Start()
    {
        TMP = GameObject.Find("Cost Text").GetComponent<TextMeshProUGUI>();
        FI = flower.GetComponent<flowerInfo>();
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
}
