using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerController : MonoBehaviour
{
    public GameObject sunFlower;
    public GameObject WaterFlower;
    public GameObject NutrientFlower;
    public GameObject RadioActiveFlower;

    public enum flowerType { Sun, Water, Nutrient, RadioActive };

    public Dictionary<flowerType, GameObject> flowerTypes = new Dictionary<flowerType, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        flowerTypes.Add(flowerType.Sun, sunFlower);
        flowerTypes.Add(flowerType.Water, WaterFlower);
        flowerTypes.Add(flowerType.Nutrient, NutrientFlower);
        flowerTypes.Add(flowerType.RadioActive, RadioActiveFlower);
    }
}
