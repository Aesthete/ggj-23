using UnityEngine;
using UnityEditor;
using System.Linq;

public class MapTool : EditorWindow
{
    public GameObject mapParent;
    public GameObject mapNodePrefab;
    public string mapData;

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
        GameObject[] objs = Selection.objects as GameObject[];

        if (GUILayout.Button("Create Links"))
            link();

        mapData = GUILayout.TextArea(mapData);
        mapNodePrefab = EditorGUILayout.ObjectField("Map node Prefab", mapNodePrefab, typeof(GameObject), false) as GameObject;
        mapParent = EditorGUILayout.ObjectField("Map Root Prefab", mapParent, typeof(GameObject), false) as GameObject;
        if (GUILayout.Button("Generate Map"))
        {
            if (mapParent == null)
                EditorUtility.DisplayDialog("No Map Parent", "Specify a game object for map parent type.", "ok", "You're not my real dad!");
            GameObject p = Instantiate(mapParent);
            p.name = "Map Parent";
            generateMap(p, mapData);
        }
    }

    void generateMap(GameObject parent, string data)
    {
        string[] lines = data.Split("\n");
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            GameObject go = Instantiate(mapNodePrefab);
            go.transform.parent = parent.transform;
            MapNode mapNode = go.GetComponent<MapNode>();
            mapNode.nodeId = i;
            string[] lineData = line.Trim().Split(",");
            float x = float.Parse(lineData[0]);
            float y = float.Parse(lineData[1]);
            go.transform.position = new Vector3(x * 20, y * 20, 0);
            for (int ii = 2; ii < lineData.Length; ii++)
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
