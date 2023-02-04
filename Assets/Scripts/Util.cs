using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Util
{
    static public uint GetSegmentsBetweenNodes(MapNode a, MapNode b)
    {
        float dist = (a.transform.position - b.transform.position).magnitude;
        return (uint)Mathf.Max(dist / (float)Globals.SegmentSize, 1f);
    }
}
