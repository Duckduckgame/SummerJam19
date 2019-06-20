using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class tickerController : MonoBehaviour
{
    public TextMeshProUGUI sunText;

    public int sunAmount = 1;

    public int sunFlower01Amount = 1;

    public int tickerCooldown = 10;

    public List<GameObject> flowers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("tickFlowers", 0, tickerCooldown);

        
        
    }

    // Update is called once per frame
    void Update()
    {
        sunText.text = "Sun: " + sunAmount.ToString();
    }

    void tickFlowers() {

        foreach (GameObject flower in flowers)
        {
            Debug.Log(flower.GetComponent<flowerInfo>().crntSunPerTick);
            sunAmount += flower.GetComponent<flowerInfo>().crntSunPerTick;
        }
    }
}
