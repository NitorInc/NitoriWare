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
        public MaskPuzzleMaskFragment fragment1;
        public MaskPuzzleMaskFragment fragment2;
    }

    // Check if two fragments can be connected directly
    public bool areConnectable(MaskPuzzleFragmentGroup group1, MaskPuzzleFragmentGroup group2)
    {
        if (overrideEdgeCheck)
            return true;
        foreach (Edge edge in connectableEdges)
        {
            if ((group1 == edge.fragment1.fragmentGroup && group2 == edge.fragment2.fragmentGroup) ||
                (group1 == edge.fragment2.fragmentGroup && group2 == edge.fragment1.fragmentGroup))
            {
                return true;
            }
        }
        return false;
    }
}
