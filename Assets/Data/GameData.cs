using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "NewGameData", menuName = "GGJ/Game Data")]
public class GameData : ScriptableObject
{
    public List<int> buildingUpgradeCosts = new List<int>();
}
