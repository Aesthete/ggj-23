using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    public Producer parent;
    [HideInInspector]
    public Building origin;
    [HideInInspector]
    public Building destination;

    private List<MapNode> path = new List<MapNode>();

    public string transporterName;
    public int nodesTravelledPerDay;
    public int holdCapacity;
    public int weightCapacity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
