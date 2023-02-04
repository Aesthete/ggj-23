using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "NewGoodData", menuName = "GGJ/Good Data")]
public class GoodData : ScriptableObject
{
    public enum GoodType { None, Vegetable, Fabric, Rocks }
    public GoodType type;
    public string goodName;
    public int value;
    public int weight;
}
