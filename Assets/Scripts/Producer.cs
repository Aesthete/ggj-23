using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Producer : MonoBehaviour
{
    public enum UpgradeOption
    {
        IncreaseProduction,
        IncreaseCapacity,
        AddTransporter,
        UpgradeTransporter
    };

    public GoodData.GoodType productionType;
    public int level;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
