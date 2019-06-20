using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tickerController : MonoBehaviour
{

    public int sunAmount = 0;

    public int sunFlower01Amount = 1;

    public int tickerCooldown = 10;

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
        sunAmount += sunFlower01Amount * 1;
        Debug.Log(sunAmount.ToString());
        Debug.Log(Time.time);
    }
}
