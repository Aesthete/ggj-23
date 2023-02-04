using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{

    public List<MapNode> mapNodes;

    // Start is called before the first frame update
    void Start()
	{ 
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<MapNode> GetShortestPath(MapNode start, MapNode end)
    {
        List<MapNode> path = new List<MapNode>();
        //path.Add(start);


        List<MapNode> unvisited = new List<MapNode>();
        Dictionary<MapNode, MapNode> previous = new Dictionary<MapNode, MapNode>();
        Dictionary<MapNode, int> distances = new Dictionary<MapNode, int>();

        foreach (MapNode node in mapNodes)
        {
            unvisited.Add(node);
            distances.Add(node, int.MaxValue);
        }

        distances[start] = 0;
        while (unvisited.Count != 0)
        {
			// Ordering the unvisited list by distance, smallest distance at start and largest at end
			unvisited = unvisited.OrderBy(node => distances[node]).ToList();

			// Getting the Node with smallest distance
			MapNode current = unvisited[0];

			// Remove the current node from unvisisted list
			unvisited.Remove(current);

			// When the current node is equal to the end node, then we can break and return the path
			if (current == end)
			{

				// Construct the shortest path
				while (previous.ContainsKey(current))
				{

					// Insert the node onto the final result
					path.Insert(0, current);

					// Traverse from start to end
					current = previous[current];
				}

				// Insert the source onto the final result
				path.Insert(0, current);
				break;
			}

			// Looping through the Node connections (neighbors) and where the connection (neighbor) is available at unvisited list
			foreach (MapNode sibling in current.siblings)
			{
				// Getting the distance between the current node and the connection (neighbor)
				//float length = Vector3.Distance(current.transform.position, sibling.transform.position);
				int length = 1; // All distances are 1.

				// The distance from start node to this connection (neighbor) of current node
				int alt = distances[current] + length;

				// A shorter path to the connection (neighbor) has been found
				if (alt < distances[sibling])
				{
					distances[sibling] = alt;
					previous[sibling] = current;
				}
			}
		}
		return path;
    }
}
