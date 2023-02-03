using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public List<int> siblingIndices = new List<int>();
    public List<MapNode> siblings = new List<MapNode>();
    public int nodeId;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        foreach (MapNode node in siblings)
        {
            Gizmos.color = Color.red;

            //Draw the suspension
            Gizmos.DrawLine(
                transform.position,
                node.transform.position
            );
        }
#endif
    }
}
