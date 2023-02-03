using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "NewBuildingData", menuName = "GGJ/Building Data")]
public class BuildingData : ScriptableObject
{
    public List<int> cost;
    public ConsumerData consumerData;
}
