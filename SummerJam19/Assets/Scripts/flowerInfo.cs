using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerInfo : MonoBehaviour
{

    public float sunPerTick01;
    public float waterPerTick01;
    public float nutrientPerTick01;

    public float sunPerTick02;
    public float waterPerTick02;
    public float nutrientPerTick02;

    public float sunPerTick03;
    public float waterPerTick03;
    public float nutrientPerTick03;

    public float crntSunPerTick;
    public float crntWaterPerTick;
    public float crntNutrientPerTick;

    public GameObject upgrade02;
    public GameObject upgrade03;

    public enum upgradeLevel {one, two, three}

    public upgradeLevel crntUpgradeLvl = upgradeLevel.one;

    public int localWaterAmount = 0;

    public int localNutrientAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        crntSunPerTick = sunPerTick01;
        crntNutrientPerTick = nutrientPerTick01;
        crntWaterPerTick = waterPerTick01;
    }

    public void upgradeFlower() {
        if (crntUpgradeLvl == upgradeLevel.two) {
            crntUpgradeLvl = upgradeLevel.three;
            upgrade03 = Instantiate(upgrade03, transform.position, Quaternion.identity, transform);
            upgrade02.GetComponent<MeshRenderer>().enabled = false;

            crntSunPerTick = sunPerTick03;
            crntWaterPerTick = waterPerTick03;
            crntNutrientPerTick = nutrientPerTick03;
        }

        if (crntUpgradeLvl == upgradeLevel.one) { 
            crntUpgradeLvl = upgradeLevel.two;

            upgrade02 = Instantiate(upgrade02, transform.position, Quaternion.identity, transform);
            GetComponent<MeshRenderer>().enabled = false;

            crntSunPerTick = sunPerTick02;
            crntWaterPerTick = waterPerTick02;
            crntNutrientPerTick = nutrientPerTick02;
            }
        
    }
}
