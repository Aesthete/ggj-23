using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public List<int> siblingIndices = new List<int>();
    public List<MapNode> siblings = new List<MapNode>();
    public int nodeId;
    public bool locked = true;
    public bool startNode = false;

    public int unlockGroup = 0;

    public GameObject island;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBuildBuilding()
    {
        island.SetActive(false);

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

        if (MapTool.showGroupColors)
        {
            Random.State state = Random.state;
            Random.InitState(unlockGroup);
            Color groupColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            Random.state = state;

            if (locked)
            {
                Gizmos.color = groupColor;
                Gizmos.DrawIcon(transform.position, "lock.jpeg", true, groupColor);
            }

            GetComponentInChildren<SpriteRenderer>().color = groupColor;
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
#endif
    }
}
