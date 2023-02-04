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

    void Start()
    {
        map = FindObjectOfType<Map>();
        ResetGame();
    }

    private void ResetGame()
    {
        foreach(Building go in FindObjectsOfType<Building>())
        {
            Destroy(go.gameObject);
        }
        placeStartingBuildings();
        placeRandomTestBuildings();
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
    }

    // Update is called once per frame
    void Update()
    {
    }
}
