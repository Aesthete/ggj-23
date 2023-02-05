using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    public GameObject transporterPrefab;
    public GameObject vegetableProducerPrefab;
    public GameObject fabricProducerPrefab;
    public GameObject rockProducerPrefab;
    public GameObject consumerPrefab;

    public Map map;
    public int currentDay = 0;
    public int currentTick = 0;
    Coroutine timerCoroutine;

    void Start()
    {
        map = FindObjectOfType<Map>();
        ResetGame();
    }

    IEnumerator _timer(uint seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    void OnTick()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        StartCoroutine(_timer(Globals.SecondsInTick));
        currentTick++;
        Debug.Log("Tick!");

        if (currentTick % Globals.TicksInDay == 0)
        {
            OnNewDay(currentDay, currentDay+1);
        }
        OnTick();
    }

    void OnNewDay(int currentDay, int newDay)
    {
        this.currentDay = newDay;
        Debug.Log("New day!");
    }


    private void ResetGame()
    {
        foreach(Building go in FindObjectsOfType<Building>())
        {
            Destroy(go.gameObject);
        }
        placeStartingBuildings();
        placeRandomTestBuildings();

        currentTick = 0;
        currentDay = 0;
        //OnTick();
    }

    void placeRandomTestBuildings()
    {
        List<MapNode> freeNodes = FindObjectsOfType<MapNode>()
            .Where(x => x.GetComponent<Building>() == null)
            .ToList();

        for ( int i = 0; i < 5; i++)
        {
            int idx = Random.Range(0, freeNodes.Count - 1);
            int idx2 = Random.Range(0, freeNodes.Count - 1);

            if (idx != idx2)
                PlaceProducerConsumerPair(freeNodes[idx], freeNodes[idx2]);

            freeNodes.RemoveAt(idx);
            freeNodes.RemoveAt(idx2);
        }

    }

    void placeStartingBuildings()
    {
        MapNode[] startingNodes = FindObjectsOfType<MapNode>()
            .Where(x => x.startNode)
            .ToArray();

        Debug.Assert(startingNodes.Length >= 2, "Not enought starting nodes to create map.");

        MapNode consumerNode = startingNodes[0];
        MapNode producerNode = startingNodes[1];

        PlaceProducerConsumerPair(startingNodes[0], startingNodes[1]);
    }

    void PlaceProducerConsumerPair(MapNode c, MapNode p)
    {
        GameObject consumer = Instantiate(consumerPrefab, c.transform);
        GameObject producer = Instantiate(vegetableProducerPrefab, p.transform);

        consumer.GetComponent<Consumer>().OnBuild();
        producer.GetComponent<Producer>().OnBuild();

        c.OnBuildBuilding();
        p.OnBuildBuilding();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
