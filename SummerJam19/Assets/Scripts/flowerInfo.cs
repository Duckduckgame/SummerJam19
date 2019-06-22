using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerInfo : MonoBehaviour
{

    [Header("Per Tick Amounts 01")]
    public float sunPerTick01;
    public float waterPerTick01;
    public float nutrientPerTick01;
    [Header("Per Tick Amounts 02")]
    public float sunPerTick02;
    public float waterPerTick02;
    public float nutrientPerTick02;
    [Header("Per Tick Amounts 03")]
    public float sunPerTick03;
    public float waterPerTick03;
    public float nutrientPerTick03;

    [System.NonSerialized]
    public float crntSunPerTick;
    [System.NonSerialized]
    public float crntWaterPerTick;
    [System.NonSerialized]
    public float crntNutrientPerTick;

    [Header("Base cost")]
    public float baseSunCost;
    public float baseWaterCost;
    public float baseNutrientCost;
    [Header("upgrade 01 cost")]
    public float u1SunCost;
    public float u1WaterCost;
    public float u1NutrientCost;
    [Header("upgrade 02 cost")]
    public float u2SunCost;
    public float u2WaterCost;
    public float u2NutrientCost;

    [Header("upgrade models")]
    public GameObject upgrade02;
    public GameObject upgrade03;

    public enum upgradeLevel {one, two, three}

    public upgradeLevel crntUpgradeLvl = upgradeLevel.one;

    public int localWaterAmount = 0;

    public int localNutrientAmount = 0;

    public flowerController.flowerType crntFlowerType;

    tickerController TC;

    // Start is called before the first frame update
    void Start()
    {
        TC = GameObject.Find("TickerController").GetComponent<tickerController>();

        crntSunPerTick = sunPerTick01;
        crntNutrientPerTick = nutrientPerTick01;
        crntWaterPerTick = waterPerTick01;
    }

    public void upgradeFlower() {

            if (crntUpgradeLvl == upgradeLevel.two)
            {
                crntUpgradeLvl = upgradeLevel.three;
                upgrade03 = Instantiate(upgrade03, transform.position, transform.rotation, transform);
                upgrade02.GetComponent<MeshRenderer>().enabled = false;

                crntSunPerTick = sunPerTick03;
                crntWaterPerTick = waterPerTick03;
                crntNutrientPerTick = nutrientPerTick03;

                TC.sunAmount -= u2SunCost;
                TC.waterAmount -= u2WaterCost;
                TC.nutrientAmount -= u2NutrientCost;
            return;
            }

            if (crntUpgradeLvl == upgradeLevel.one)
            {
                crntUpgradeLvl = upgradeLevel.two;

                upgrade02 = Instantiate(upgrade02, transform.position, transform.rotation, transform);
                GetComponent<MeshRenderer>().enabled = false;

                crntSunPerTick = sunPerTick02;
                crntWaterPerTick = waterPerTick02;
                crntNutrientPerTick = nutrientPerTick02;


            TC.sunAmount -= u1SunCost;
            TC.waterAmount -= u1WaterCost;
            TC.nutrientAmount -= u1NutrientCost;
        }
        
    }
}
