using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Button placePlants;


    // Start is called before the first frame update
    void Start()
    {
        placePlants.onClick.AddListener(checkPlantPlaces);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void checkPlantPlaces() {
        Debug.Log("click!");
    }
}
