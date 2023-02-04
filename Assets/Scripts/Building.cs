using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData buildingData;
    Consumer consumer;
    Producer producer;

    // Start is called before the first frame update
    void Start()
    {
        consumer = GetComponentInChildren<Consumer>();
        producer = GetComponentInChildren<Producer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
