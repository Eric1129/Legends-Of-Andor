using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public class Graph
{

    private Dictionary<int, Node> nodes;


    // protected constructor since this class is a singleton.
    // ie only children can call the constructor
    public Graph()
    {
        nodes = new Dictionary<int, Node>();
        loadData();
    }

    private void loadData()
    {
        readGraphCSV();
    }


    private void readGraphCSV()
    {
        int[][] fileContents = new int[85][];
        int maxSize = 0;

        using(var reader = new StreamReader(@"./Assets/CSV/adjacencyList.txt"))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine().ToString().TrimEnd( Environment.NewLine.ToCharArray());
                string[] indices = line.Split(',');

                if (maxSize < indices.Length)
                {
                    maxSize = indices.Length;
                }

                int index = Int32.Parse(indices[0]);

                fileContents[index] = toIntArray(indices);
            }
        }

        for(int index = 0; index < fileContents.Length; index++)
        {
            if (fileContents[index] != null)
            {
                nodes.Add(index, new Node(index));
            }
        }

        for (int index = 0; index < fileContents.Length; index++)
        {
            if (fileContents[index] != null)
            {
                for(int x = 1; x < fileContents[index].Length; x++)
                {
                    nodes[fileContents[index][0]].addAdjacentNode(nodes[fileContents[index][x]]);
                }
            }
        }
    }

    public int getDistance(int src, int dest)
    {
        Node srcNode = nodes[src];
        Node destNode = nodes[dest];

        // destNode has its attribute: prev set correctly now
        // Node destNode = ref bfs(nodes[srcIndex], nodes[destIndex]);
        return bfs(srcNode, destNode).Count;
    }

    private List<Node> bfs(Node src, Node dest)
    {
        if(src == dest)
        {
            return new List<Node>();
        }
        Queue<List<Node>> queue = new Queue<List<Node>>();
        List<Node> firstNode = new List<Node>();
        firstNode.Add(src);
        queue.Enqueue(firstNode);        

        while (queue.Count != 0)
        {
            List<Node> path = queue.Dequeue();
            Node lastNode = path[path.Count-1];

            // if notLooking for castle zero then don't consider as neighbour??
            // can we go thru the castle space if convenient?
            if (lastNode.getIndex() == dest.getIndex())
            {
                return path;
            }

            foreach(Node adjNode in lastNode.getAdjacentNodes())
            {
                if (path.Contains(adjNode)) { continue; }

                // Copy the list
                List<Node> listCopy = new List<Node>(path.ToArray());

                listCopy.Add(adjNode);
                queue.Enqueue(listCopy);
            }
        }
        // No valid path
        return null;
        // return ref new Node(0, new int[] {-1, -1, -1});
    }



// ------ some helper functions -----
    private int[] toIntArray(string[] stringArr)
    {
        int[] intArr = new int[stringArr.Length];
        for (int i = 0; i < stringArr.Length; i++)
        {
            intArr[i] = Int32.Parse(stringArr[i]);
        }
        return intArr;
    }
}
