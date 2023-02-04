using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    [HideInInspector]
    public Producer parent;
    [HideInInspector]
    public Building origin;
    [HideInInspector]
    public Building destination;

    private List<MapNode> path = new List<MapNode>();
    MapNode currentLocation;
    Map map;

    public string transporterName;
    public uint nodesTravelledPerDay;
    public uint holdCapacity;
    public uint weightCapacity;

    // Start is called before the first frame update
    void Start()
    {
        map = FindObjectOfType<Map>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveTransportOnPath(List<MapNode> path)
    {
        StartCoroutine(MoveOnPath(path));
    }

    IEnumerator MoveOnPath(List<MapNode> path)
    {
        float secondsPerNode = Globals.SecondsInDay / (float)nodesTravelledPerDay;
        currentLocation = path[0];
        transform.position = currentLocation.transform.position;
        path.RemoveAt(0);

        foreach (MapNode node in path)
        {
            yield return StartCoroutine(MoveFromTo(transform.position, node.transform.position, secondsPerNode));
        }
    }

    IEnumerator MoveFromTo(Vector3 a, Vector3 b, float secondsTotal)
    {
        float step = (secondsTotal * (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            transform.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        transform.position = b;
    }
}
