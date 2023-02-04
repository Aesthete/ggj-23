using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "NewTransporterData", menuName = "GGJ/Transporter Data")]
public class TransporterData : ScriptableObject
{
    public string transporterName;
    public int timeBetweenNodesMilliseconds;
    public int holdCapacity;
    public int weightCapacity;
}