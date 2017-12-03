using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPuzzleMaskEdges : MonoBehaviour
{
    [Header("Ignore the edges and always allow two mask pieces to connect")]
    public bool overrideEdgeCheck = true;

    [Header("All possible edges that can connect with one-another")]
    public Edge[] connectableEdges;

    [System.Serializable]
    public class Edge
    {
        public Transform fragment1;
        public Transform fragment2;
    }

    // Check if two fragments can be connected directly
    public bool areConnectable(Transform fragment1, Transform fragment2)
    {
        if (overrideEdgeCheck)
            return true;
        foreach (Edge edge in connectableEdges)
        {
            if ((fragment1 == edge.fragment1 && fragment2 == edge.fragment2) ||
                (fragment1 == edge.fragment2 && fragment2 == edge.fragment1))
            {
                return true;
            }
        }
        return false;
    }

    // Check if any fragment from the first group can be connected to any fragment from the second group
    public bool areConnectable(List<GameObject> fragmentGroup1, List<GameObject> fragmentGroup2)
    {
        foreach (GameObject fragment1 in fragmentGroup1)
            foreach (GameObject fragment2 in fragmentGroup2)
                if (areConnectable(fragment1.transform, fragment2.transform))
                    return true;

        return false;
    }
}
