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
}
