using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapTool : EditorWindow
{
    [MenuItem("GGJ/Map Tool")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MapTool window = (MapTool)EditorWindow.GetWindow(typeof(MapTool));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Map Tool", EditorStyles.boldLabel);
        GameObject[] objs = Selection.objects as GameObject[];

        if (GUILayout.Button("Create Links"))
            link();
    }

    void OnDrawGizmos()
    {
        drawLinks();
    }

    void drawLinks()
    {
        foreach (MapNode node in GameObject.FindObjectsOfType<MapNode>())
        {
            foreach (MapNode sibling in node.siblings)
            {
                Gizmos.DrawLine(node.transform.position, sibling.transform.position);
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
