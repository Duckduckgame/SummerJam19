using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public float lerpSpeed;

    public tileScript TS;
    public flowerInfo FI;

    public Button placeSunBut;
    public Button placeWaterBut;
    public Button placeNutriBut;
    public Button placeRadioBut;
    public Button triggerBut;

    public Button upgradeBut;

    TextMeshProUGUI costText;

    public CanvasGroup flowerPlace;

    public CanvasGroup triggerPlace;

    public CanvasGroup flowerUpgrade;

    public CanvasGroup pause;

    public CanvasGroup none;

    public enum UIType {None, FlowerPlace, FlowerUpgrade, TriggerPlace, Pause }

    public UIType crntType = UIType.None;

    UIType oldType = UIType.None;

    Dictionary<UIType, CanvasGroup> UISections;

    bool isfading;

    // Start is called before the first frame update
    void Start()
    {

        
        TS = GameObject.Find("TileManager").GetComponent<tileScript>();

        flowerPlace = GameObject.Find("FlowerPlacementGroup").GetComponent<CanvasGroup>();
        none = GameObject.Find("NoneGroup").GetComponent<CanvasGroup>();
        flowerUpgrade = GameObject.Find("FlowerUpgradeGroup").GetComponent<CanvasGroup>();
        pause = GameObject.Find("PauseGroup").GetComponent<CanvasGroup>();
        triggerPlace = GameObject.Find("triggerPlacementGroup").GetComponent<CanvasGroup>();

        costText = GameObject.Find("Cost Text").GetComponent<TextMeshProUGUI>();

        UISections = new Dictionary<UIType, CanvasGroup>();
        UISections.Add(UIType.None, none);
        UISections.Add(UIType.FlowerPlace, flowerPlace);
        UISections.Add(UIType.FlowerUpgrade, flowerUpgrade);
        UISections.Add(UIType.Pause, pause);
        UISections.Add(UIType.TriggerPlace, triggerPlace);

        placeSunBut.onClick.AddListener(placeSun);
        placeNutriBut.onClick.AddListener(placeNutrient);
        placeRadioBut.onClick.AddListener(placeRadioactive);
        placeWaterBut.onClick.AddListener(placeWater);
        triggerBut.onClick.AddListener(placeTrigger);



        upgradeBut.onClick.AddListener(upgradeFlower);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (crntType != oldType) {
            Debug.Log(crntType);
            switchUIType();
            oldType = crntType;
            
        }
    }

    void placeSun() {
        TS.placeFlower(flowerController.flowerType.Sun);
    }
    void placeWater()
    {
        TS.placeFlower(flowerController.flowerType.Water);
    }
    void placeNutrient()
    {
        TS.placeFlower(flowerController.flowerType.Nutrient);
    }
    void placeRadioactive()
    {
        TS.placeFlower(flowerController.flowerType.RadioActive);
    }
    void placeTrigger()
    {
        TS.placeTrigger();
    }

    void upgradeFlower() {
        TS.mapToFlower[TS.selectedTile].GetComponent<flowerInfo>().upgradeFlower();
    }

    public void switchUIType() {

        CanvasGroup oldCG = UISections[oldType];
        CanvasGroup newCG = UISections[crntType];


        StartCoroutine(FadeCG(oldCG, newCG));

        oldCG.interactable = false;
        oldCG.blocksRaycasts = false;

        newCG.interactable = true;
        newCG.blocksRaycasts = true;
        }

    IEnumerator FadeCG(CanvasGroup oldCG, CanvasGroup newCG) {
        for (float f = 1f; f >= 0; f -= 0.001f)
        {

            oldCG.alpha = f;
            newCG.alpha = 1-f;
        }
        yield return null;
    }


}
