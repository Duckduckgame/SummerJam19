using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tickerController : MonoBehaviour
{

    public int sunAmount = 0;

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

    }

    void tickFlowers() {

        foreach (GameObject flower in flowers)
        {
            sunAmount += flower.GetComponent<flowerInfo>().crntSunPerTick;
        }
    }
}
