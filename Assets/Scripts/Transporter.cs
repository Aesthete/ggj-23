using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Transporter : MonoBehaviour
{
    public Producer parent;
    public Building destination;

    private List<MapNode> path = new List<MapNode>();
    MapNode currentLocation;
    Map map;

    public string transporterName;
    public uint nodesTravelledPerDay;
    public uint holdCapacity;
    public uint weightCapacity;

    Coroutine movementRoutine;

    // Start is called before the first frame update
    void Start()
    {
        map = FindObjectOfType<Map>();
        this.transform.position = parent.transform.position;
        currentLocation = parent.GetComponent<MapNode>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestinationReached(List<MapNode> path)
    {
        Debug.Log("Reached!");
        int idx = Random.Range(0, map.mapNodes.Count - 1);
        path = map.GetShortestPath(currentLocation, map.mapNodes[idx]);
        MoveTransportOnPath(path);
    }

    public void MoveTransportOnPath(List<MapNode> path)
    {
        if (movementRoutine != null) StopCoroutine(movementRoutine);
        movementRoutine = StartCoroutine(MoveOnPath(path));
    }

    IEnumerator MoveOnPath(List<MapNode> path)
    {
        float secondsPerNode = Globals.SecondsInDay / (float)nodesTravelledPerDay;
        currentLocation = path[0];
        transform.position = currentLocation.transform.position;

        foreach (MapNode node in path.Skip(1))
        {
            yield return StartCoroutine(MoveFromTo(currentLocation, node, secondsPerNode));
        }
        OnDestinationReached(path);
    }

    IEnumerator MoveFromTo(MapNode a, MapNode b, float secondsTotal)
    {
        float step = (secondsTotal * (a.transform.position - b.transform.position).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            transform.position = Vector3.Lerp(a.transform.position, b.transform.position, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        currentLocation = b;
        transform.position = b.transform.position;
    }
}
