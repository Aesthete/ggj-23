using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Transporter : MonoBehaviour
{
    public Producer parent;

    private List<MapNode> path = new List<MapNode>();
    MapNode currentLocation;
    MapNode currentDestination;
    public Map map;

    public string transporterName;
    public uint segmentsTravelledPerDay;
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

    void OnDestinationReached()
    {
        Debug.Log("Reached!");
        currentDestination = this.path.First();
        this.path = map.GetShortestPath(currentLocation, currentDestination);
        MoveTransportOnPath(path);
    }

    public void DeliverTo(MapNode target, MapNode forceStartFrom = null)
    {
        currentDestination = target;

        if (forceStartFrom != null)
            currentLocation = forceStartFrom;
        List<MapNode> path = map.GetShortestPath(currentLocation, currentDestination);
        MoveTransportOnPath(path);
    }

    public void MoveTransportOnPath(List<MapNode> path)
    {
        if (movementRoutine != null) StopCoroutine(movementRoutine);
        movementRoutine = StartCoroutine(MoveOnPath(path));
    }

    IEnumerator MoveOnPath(List<MapNode> path)
    {
        this.path = path;
        currentLocation = path[0];
        transform.position = currentLocation.transform.position;

        foreach (MapNode node in path.Skip(1))
        {
            uint segments = Util.GetSegmentsBetweenNodes(currentLocation, node);
            float secondsPerSegment = (Globals.SecondsInDay / segmentsTravelledPerDay);
            float time = segments * secondsPerSegment;
            yield return StartCoroutine(MoveFromTo(currentLocation, node, time));
        }
        OnDestinationReached();
    }

    IEnumerator MoveFromTo(MapNode a, MapNode b, float timeTaken)
    {
        float step = (Time.fixedDeltaTime / timeTaken);
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
