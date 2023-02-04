using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "NewBuildingData", menuName = "GGJ/Building Data")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public List<int> upgradeCosts;

    public GoodData.GoodType produces;
    public List<int> productionAmount;
    public List<int> productionSpeedPerDay;

    public GoodData.GoodType consumes;
    public List<int> consumptionAmount;
    public List<int> patienceLostPerDay;
}
