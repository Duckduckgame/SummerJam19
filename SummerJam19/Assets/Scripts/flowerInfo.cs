using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerInfo : MonoBehaviour
{

    public int sunPerTick01;
    public int waterPerTick01;
    public int nutrientPerTick01;

    public int sunPerTick02;
    public int waterPerTick02;
    public int nutrientPerTick02;

    public int sunPerTick03;
    public int waterPerTick03;
    public int nutrientPerTick03;

    public int crntSunPerTick;
    public int crntWaterPerTick;
    public int crntNutrientPerTick;

    public enum upgradeLevel {one, two, three}

    public upgradeLevel crntUpgradeLvl = upgradeLevel.one;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
