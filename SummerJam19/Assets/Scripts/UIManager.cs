using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float lerpSpeed;

    public tileScript TS;

    public Button placePlants;

    public CanvasGroup flowerPlace;

    public CanvasGroup flowerUpgrade;

    public CanvasGroup pause;

    public CanvasGroup none;

    public enum UIType {None, FlowerPlace, FlowerUpgrade, Pause }

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

        UISections = new Dictionary<UIType, CanvasGroup>();
        UISections.Add(UIType.None, none);
        UISections.Add(UIType.FlowerPlace, flowerPlace);
        UISections.Add(UIType.FlowerUpgrade, flowerUpgrade);
        UISections.Add(UIType.Pause, pause);

        placePlants.onClick.AddListener(placeFlower);   
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

    void placeFlower() {
        TS.placeFlower();
    }

    void switchUIType() {

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
