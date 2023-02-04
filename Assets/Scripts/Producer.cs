using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Producer : MonoBehaviour
{
    public enum UpgradeOption
    {
        IncreaseProduction,
        IncreaseCapacity,
        AddTransporter,
        UpgradeTransporter
    };

    public GoodData.GoodType productionType;
    public int level;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBuild()
    {
        AddTransporter(1);
    }

    private void AddTransporter(int level)
    {
        GameObject tpPrefab = FindObjectOfType<GameController>().transporterPrefab;
        GameObject tp = Instantiate(tpPrefab, this.transform);
        Transporter transporter = tp.GetComponent<Transporter>();

        transporter.parent = this;
        Map map = FindObjectOfType<Map>();
        MapNode ourNode = this.GetComponentInParent<MapNode>();
        Consumer closest = map.FindClosest<Consumer>(ourNode);
        transporter.map = map;
        transporter.DeliverTo(closest.GetComponentInParent<MapNode>(), ourNode);
    }
}
