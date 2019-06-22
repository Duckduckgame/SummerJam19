using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class triggerText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI TMP;
    tickerController TC;
    Button but;

    // Start is called before the first frame update
    void Start()
    {
        TMP = GameObject.Find("Cost Text").GetComponent<TextMeshProUGUI>();
        TC = GameObject.Find("TickerController").GetComponent<tickerController>();
        but = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TC.sunAmount >= 500 && TC.waterAmount >= 500 && TC.nutrientAmount >= 500)
        {
            but.interactable = true;
        }

        if (TC.sunAmount < 500 || TC.waterAmount < 500 || TC.nutrientAmount < 500)
        {
            but.interactable = false;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {

        TMP.transform.position = Input.mousePosition + new Vector3(150, 0, 0);
        TMP.text = "Sun: 500 Water: 500 Nutrient: 500";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TMP.transform.position = new Vector3(-500, 0, 0);
    }

}
