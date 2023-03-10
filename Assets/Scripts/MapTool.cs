using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class MapTool : EditorWindow
{
    public GameObject mapParent;
    public GameObject mapNodePrefab;
    public GameObject BridgePrefab;
    public string mapData;

    public static bool showGroupColors = false;

    [MenuItem("GGJ/Map Tool")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MapTool window = (MapTool)GetWindow(typeof(MapTool));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Map Tool", EditorStyles.boldLabel);

        if (GUILayout.Button("Create Links"))
            link();

        mapNodePrefab = EditorGUILayout.ObjectField("Map node Prefab", mapNodePrefab, typeof(GameObject), false) as GameObject;
        mapParent = EditorGUILayout.ObjectField("Map Root Prefab", mapParent, typeof(GameObject), false) as GameObject;
        BridgePrefab = EditorGUILayout.ObjectField("Bridge prefab", BridgePrefab, typeof(GameObject), false) as GameObject;
        if (GUILayout.Button("Generate Map"))
        {
            if (mapParent == null)
                EditorUtility.DisplayDialog("No Map Parent", "Specify a game object for map parent type.", "ok", "You're not my real dad!");
            GameObject p = Instantiate(mapParent);
            p.name = "Map Parent";
            generateMap(p, mapData);
        }
        mapData = GUILayout.TextArea(mapData);

        if (GUILayout.Button("Group Nodes"))
        {
            Object[] objs = Selection.objects;
            if (objs.Length < 1)
                return;
            MapNode[] nodes = new MapNode[2];
            foreach (GameObject obj in objs)
            {
                if (obj.GetComponent<MapNode>() == null)
                {
                    return;
                }
            }
        }

        if (GUILayout.Button("Print Path"))
        {
            Object[] objs = Selection.objects;
            if (objs.Length != 2)
                return;
            MapNode[] nodes = new MapNode[2];
            nodes[0] = (objs[0] as GameObject).GetComponent<MapNode>();
            nodes[1] = (objs[1] as GameObject).GetComponent<MapNode>();

            Map map = FindObjectOfType<Map>();
            List<MapNode> path = map.GetShortestPath(nodes[0], nodes[1]);

            Debug.Log(string.Join(" - ", path.Select(x => x.nodeId)));

            Transporter t = FindObjectOfType<Transporter>();
            t.MoveTransportOnPath(path);
        }

        if (GUILayout.Button("Set New Target"))
        {
            GameObject obj = Selection.activeGameObject;
            MapNode comp = obj.GetComponent<MapNode>();

            foreach (Transporter tp in FindObjectsOfType<Transporter>())
            {
                tp.DeliverTo(comp);
            }
        }

        showGroupColors = GUILayout.Toggle(showGroupColors, "Show group colors");
    }

    void generateMap(GameObject parent, string data)
    {
        string[] lines = data.Split("\n");
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            GameObject go = PrefabUtility.InstantiatePrefab(mapNodePrefab) as GameObject;
            go.transform.parent = parent.transform;
            parent.GetComponent<Map>().mapNodes.Add(go.GetComponent<MapNode>());
            MapNode mapNode = go.GetComponent<MapNode>();
            string[] lineData = line.Split(' ')[0].Trim().Split(",");
            int id = int.Parse(lineData[0]);
            mapNode.nodeId = id;
            mapNode.name = "Node: " + lineData[0];
            float x = float.Parse(lineData[1]) - 0.5f;
            float y = float.Parse(lineData[2]) - 0.5f;
            go.transform.position = new Vector3(x * (float)Globals.MapScale, y * (float)Globals.MapScale, 0);
            for (int ii = 3; ii < lineData.Length; ii++)
            {
                int sibling = int.Parse(lineData[ii]);
                mapNode.siblingIndices.Add(sibling);
            }
        }

        foreach (MapNode child in parent.GetComponentsInChildren<MapNode>())
        {
            foreach(int sibling_id in child.siblingIndices)
            {
                foreach (MapNode sibling in parent.GetComponentsInChildren<MapNode>().Where(x => x.nodeId == sibling_id))
                    child.siblings.Add(sibling);
            }
        }

        GameObject bridgesRoot = new GameObject("Bridges Root");
        bridgesRoot.transform.parent = parent.transform;

        foreach (MapNode child in parent.GetComponentsInChildren<MapNode>())
        {
            foreach (MapNode sibling in child.siblings)
            {
                buildBridge(child, sibling, bridgesRoot);
            }
        }
    }

    void buildBridge(MapNode a, MapNode b, GameObject bridgesRoot)
    {
        GameObject bridge = Instantiate(BridgePrefab);
        bridge.transform.parent = bridgesRoot.transform;
        float bridgeSize = bridge.GetComponentInChildren<SpriteRenderer>().bounds.size.magnitude * 0.8f;
        Vector3 targetDir = b.transform.position - a.transform.position;
        float dist = targetDir.magnitude;
        uint numBridges = (uint)Mathf.Ceil(dist / bridgeSize);
        int bridgesPlaced = 0;
        float angle = Vector3.SignedAngle(Vector3.up, targetDir, Vector3.forward);

        GameObject sectionParent = new GameObject();
        sectionParent.transform.parent = bridgesRoot.transform;

        for (int i = 0; i < numBridges; i++)
        {
            GameObject bridgePiece = Instantiate(bridge);
            bridgePiece.transform.parent = sectionParent.transform;
            bridgePiece.transform.Translate(Vector3.up * bridgesPlaced * bridgeSize, Space.Self);
            bridgesPlaced++;
        }
        sectionParent.transform.position = a.transform.position;
        sectionParent.transform.Rotate(Vector3.forward, angle);

        DestroyImmediate(bridge);
    }

    void link()
    {
        Object[] objs = Selection.objects;
        if (objs.Length != 2)
            return;
        MapNode[] nodes = new MapNode[2];
        foreach (GameObject obj in objs)
        {
            if (obj.GetComponent<MapNode>() == null)
            {
                    return;
            }
        }

        nodes[0] = (objs[0] as GameObject).GetComponent<MapNode>();
        nodes[1] = (objs[1] as GameObject).GetComponent<MapNode>();

        if (!nodes[0].siblings.Contains(nodes[1]))
            nodes[0].siblings.Add(nodes[1]);
        if (!nodes[1].siblings.Contains(nodes[0]))
            nodes[1].siblings.Add(nodes[0]);
    }
}
